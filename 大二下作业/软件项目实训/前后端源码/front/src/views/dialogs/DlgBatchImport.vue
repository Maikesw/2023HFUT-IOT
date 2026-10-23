<template>
  <DlgBasic :dialog="importDialog" :digbasic-submit="submit">
    <template slot="mainForm">
      <el-upload class="upload-demo" drag action :limit="1" :show-file-list="true" :http-request="()=>{}" :on-exceed="handleExceed" :before-upload="beforeUpload" :on-remove="()=>file={}">
        <i class="el-icon-upload" />
        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
        <div v-show="noFile" slot="tip" class="el-upload__tip" style="color:red">请上传文件！</div>
        <div v-if="isNeedExport" slot="tip" class="el-upload__tip">请先进行<span class="download">“导出”</span>或<span class="download">“全部导出”</span>操作获取模板</div>
        <div slot="tip" class="el-upload__tip">
          <span v-show="!isNeedExport" class="download" @click="downloadTemplate">下载模板</span>
          <span>导入文件不能超过5M，仅允许导入“xls”或“xlsx”格式文件</span>
        </div>
      </el-upload>
    </template>
  </DlgBasic>
</template>

<script>
import commonAPI from '@/api/file'
import { getTemplateFile } from '@/utils/common'
import IEAPI from '@/api/importAndExport'
import DlgBasic from '@/components/DlgBasic'

export default {
  name: 'DlgBatchImport',
  components: { DlgBasic },
  props: {
    importDialog: { type: Object, default: () => { } },
    templateFile: {
      type: Object, default: () => { return { id: null, name: '' } }
    },
    // 是否需要先执行导出操作
    isNeedExport: { type: Boolean, default: false },
    keyWords: { type: String, default: '' }
  },
  data() {
    return {
      file: {},
      fileFlag: false,
      dialogFull: false,
      comfimLoading: false
    }
  },
  computed: {
    noFile() {
      if (!this.file.type && this.fileFlag) return true
      return false
    }
  },
  methods: {
    // The parameter of the hook before uploading a file is the uploaded file.
    // If false or promise is returned and rejected, the upload will stop.
    // 上传文件前调用的钩子函数
    beforeUpload(file) {
      const fileTypes = ['xls', 'xlsx']
      const splitArr = file.name.split('.')
      const fileType = splitArr[splitArr.length - 1]
      const isLtSize = file.size / 1024 / 1024 < this.CONSTANT.FILE_MAX_SIZE

      if (!fileTypes.includes(fileType)) {
        return this.$message.error('上传文件只能是 xls/xlsx 格式!')
      }
      if (!isLtSize) {
        return this.$message.error(`上传图片大小不能超过 ${this.CONSTANT.FILE_MAX_SIZE} MB!`)
      }
      this.fileFlag = false
      this.file = file
    },
    // 上传文件数量超过限制时触发的钩子函数
    // Hook when the number of files exceeds the limit
    handleExceed(files, fileList) {
      this.$message.warning('当前限制选择 1 个文件，请删除后继续上传')
    },
    // 关闭弹窗
    // Close the pop-up window
    closeDialog() {
      this.importDialog.show = false
    },
    // Download the template, judge which interface to call according to different bellongpage fields,
    // and obtain the templates belonging to different pages
    async downloadTemplate() {
      const templateFile = getTemplateFile(this.keyWords, '.xlsx')
      try {
        this.importDialog.show = false
        this.fullScreenLoading()
        const content = await commonAPI.downloadFile(templateFile.id)
        this.fullScreenLoading().close()
        this.downloadFile(content, templateFile.name)
      } catch (error) {
        this.fullScreenLoading().close()
      }
    },
    // Override the default upload behavior and customize the implementation of upload
    // According to different bellongpage fields, determine which page's upload interface to call,
    // or change it to emit event to process the event on each page, depending on the requirements
    // 确认上传
    async submit() {
      if (!this.file.type) {
        return (this.fileFlag = true)
      }
      try {
        this.comfimLoading = true
        this.importDialog.show = false
        this.fullScreenLoading()
        await IEAPI.importInfo(this.keyWords, this.file)
        this.comfimLoading = false
        this.fullScreenLoading().close()
        this.$message.success('导入成功')
        this.$parent.$refs.dataTableList.$refs.table.clearSelection()
        this.$parent.$refs.dataTableList.initDataList()
      } catch (error) {
        this.fullScreenLoading().close()
      }
      this.file = {}
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";

::v-deep .el-dialog__body {
  display: flex;
  justify-content: center;
  align-items: center;
}
.el-upload__tip {
  display: flex;
  .download {
    margin-right: 5px;
    color: $--color-primary;
    cursor: pointer;
  }
}
.view-dialog {
  ::v-deep .el-dialog__body {
    max-height: unset;
    height: calc(100vh - 108px);
  }
}
</style>
