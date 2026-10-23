<template>
  <div>    
    <SimpleDialog ref="simpleDlg" :default-props="defaultMainSDProps" :simpledialog-confirm="confirm">
      <template slot="otherItems">
        <el-row>
          <el-col :span="11">
            <el-form-item label="是否发布">
              <el-switch v-model="form.publishStatus" />
            </el-form-item>
          </el-col>
          <el-col :span="11">
            <el-form-item label="轮播图展示">
              <el-switch v-model="form.channelCSlider" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="正文">
          <wang-editor v-model="form.channelCText" :value="form.channelCText" />
        </el-form-item>
        <el-form-item prop="images" label="图片上传">
          <SimpleUpload ref="imgUpload" v-model="form.images" list-type="picture-card" :up-button-info="upImgButtonInfo" :file-max-size="imgMaxSize" :multiple="allowMultiFiles" :fileAllowedTypes="imgAllowType" @handle-change="handleImgChange" />
        </el-form-item>
        <el-form-item prop="files" label="附件上传">
          <SimpleUpload ref="fileUpload" v-model="form.files" :up-button-info="upFileButtonInfo" :file-max-size="fileMaxSize" :multiple="allowMultiFiles"  @handle-change="handleFileChange" />
        </el-form-item>
      </template>
    </SimpleDialog>
  </div>
</template>

<script>

import SimpleDialog from '@/components/SimpleDialog'
import listAPI from '@/api/list'
import WangEditor from '@/components/wang-editor/index'
import CONSTANT from '@/constant'
import _ from 'lodash'
import SimpleUpload from '@/components/SimpleUpload'
export default {
  components: { SimpleDialog, WangEditor, SimpleUpload },
  props: {
  },
  data() {
    return {
      deleteFileList: [],
      form: {},
      defaultMainSDProps: {
        form: {},
        keyWord: 'BaseUser',
        formItems: [
          { name: '标题', field: 'channelCName', placeholder: '标题不能为空', type: 'input' }
        ],
        formRules: {
          channelCName: [{ required: true, message: '标题不能为空', trigger: 'blur' }]
        },
        defaultDBProps: {
          width: '80%',
          someFlags: {
            needMaxBtn: true
          },
          footButtons: { repeatAdd: { show: false }}
        }
      },
      fileMaxSize: 10,
      imgMaxSize: 1,
      imgAllowType: CONSTANT.FILE_TYPE.IMG,
      upImgButtonInfo: { name: '+', tooltip: '请上传图片文件，单个文件大小不超过1M。' },
      upFileButtonInfo: { name: '点击上传附件', tooltip: '可以上传各种类型的文件，但单个文件大小不超过10M。' },
      allowMultiFiles: true
    }
  },  
  methods: {
    async showDialog(val, form = {}) {
      this.form = _.cloneDeep(form) // 打开的时候用cloneDeep，这样外层传的时候就直接传row就可以了
      this.$refs.simpleDlg.showDialog(val, this.form, true)
    },
    handleImgChange(imgList) {
      this.$set(this.form, 'images', imgList)    
      this.$refs.simpleDlg.verifyValid()
    },
    handleFileChange(fileList) {      
      this.$set(this.form, 'files', fileList)
      this.$refs.simpleDlg.verifyValid()
    },
    async confirm(option, type, form) {      
      try {
        const resp = await listAPI.editOneNode('MainChannelContent', form)
        await this.$refs.imgUpload.handleUpload(this.$store.getters.userInfo.id, resp.data.id, 1)
        await this.$refs.fileUpload.handleUpload(this.$store.getters.userInfo.id, resp.data.id, 2)
        setTimeout(() => {
          // 阻止异步刷新
          this.$refs.simpleDlg.$parent.$parent.initDataList()
        }, 700)
      } catch (e) {
        console.log(e)
      }
    }
  }
}
</script>
