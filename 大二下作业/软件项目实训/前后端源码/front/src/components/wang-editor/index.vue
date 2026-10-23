<template>
  <div class="editor-box">
    <div ref="editor" class="editor"  />
  </div>
</template>

<script>
import E from 'wangeditor'
import commonAPI from '@/api/file'

export default {
  name: 'Editor',  
  props: {
    value: { type: String, default: '' },
    placeholder: { type: String, default: '请输入正文' },
    isDisabled: { type: Boolean, default: false },
    isClear: { type: Boolean, default: false }
  },
  data() {
    return {      
      editor: ''      
    }
  },
  watch: {
    isClear(val) {
      // 触发清除文本域内容
      if (val) {
        this.editor.txt.clear()
        this.info_ = null
      }
    },
    value(value) {
      if (value !== this.editor.txt.html()) {
        this.editor.txt.html(this.value)
      }
    }
    // value为编辑框输入的内容，这里我监听了一下值，当父组件调用得时候，如果给value赋值了，子组件将会显示父组件赋给的值
  },
  mounted() {
    this.setEditor()
  },
  methods: {
    setEditor() {
      this.editor = new E(this.$refs.editor)
      this.editor.config.menus = [
        'head', // 标题
        'bold', // 粗体
        'fontSize', // 字号
        'fontName', // 字体
        'italic', // 斜体
        'underline', // 下划线
        'strikeThrough', // 删除线
        'indent', // 缩进
        'lineHeight', // 行高
        'foreColor', // 文字颜色
        'backColor', // 背景颜色
        'link', // 插入链接
        'list', // 列表
        'justify', // 对齐方式
        'quote', // 引用
        'emoticon', // 表情
        'image', // 插入图片
        'table', // 插入表格
        'splitLine', // 插入分割线
        'undo', // 撤销
        'redo', // 恢复
        'fullscreen' // 全屏
      ]
      this.editor.config.placeholder = this.placeholder
      this.editor.config.uploadImgMaxSize = 10 * 1024 * 1024 // 将图片大小限制为 10M
      // editor.config.uploadFileName = 'file' // 后端接受上传文件的参数名
      // editor.config.uploadImgParams = {
      //   relationId: 0,
      //   type: 0
      // } // 上传图片时可自定义传递一些参数，参数会被添加到 formData 中，一起上传到服务端
      this.editor.config.uploadImgMaxLength = 6 // 限制一次最多上传 1 张图片
      this.editor.config.showLinkImg = false // 隐藏网络图片上传
      // 自定义上传方法
      // eslint-disable-next-line space-before-function-paren
      this.editor.config.customUploadImg = async (resultFiles, insertImgFn) => {
        // resultFiles 是 input 中选中的文件列表
        for (var file of resultFiles) {
          const params = {
            file: file,
            relationId: 0,
            type: 0,
            source: 0
          }
          const res = await commonAPI.commonUpload(params)
          const imgUrl = res.data.url
          // insertImgFn 是获取图片 url 后，插入到编辑器的方法
          insertImgFn(imgUrl)
        }
      }
      this.editor.config.uploadImgHooks = {
        fail: (xhr, editor, result) => {
          // 插入图片失败回调
          console.log(result)
        },
        success: (xhr, editor, result) => {
          // 图片上传成功回调
          console.log(result)
        },
        timeout: (xhr, editor) => {
          // 网络超时的回调
          console.log('网络超时')
        },
        error: (xhr, editor) => {
          // 图片上传错误的回调
          console.log('上传错误')
        },
        // 回显
        customInsert: (insertImg, result, editor) => {
          // 图片上传成功，插入图片的回调
          // result为上传图片成功的时候返回的数据，这里我打印了一下发现后台返回的是data：[{url:"路径的形式"},...]
          insertImg(result.data.url) // 为插入图片的函数
        }
      }
      this.editor.config.onchange = html => {
        this.$emit('input', html)
      }
      this.editor.create()
      this.editor.txt.html(this.value)
      if (this.isDisabled) this.editor.disable()
    }
  }
}
</script>

<style lang="scss" scoped>
.editor {
  ::v-deep .w-e-toolbar,
  ::v-deep .w-e-text-container {
    border-color: #ccc !important;
  }
}
</style>
