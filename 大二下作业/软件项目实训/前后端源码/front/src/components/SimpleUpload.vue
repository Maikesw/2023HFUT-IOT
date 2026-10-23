<template>
  <div>
    <el-upload ref="upload" v-model="fileList" action multiple  :list-type="listType" :auto-upload="autoUpload" :before-remove="handleBeforeRemove" :on-preview="handlePreview" :on-remove="handleRemove" :on-change="handleChange" :file-list="fileList" :http-request="() => {}" :before-upload="beforeUploadVerify">
      <el-button size="small" type="primary" :title="upButtonInfo.tooltip" @click.stop="beforeUpload">{{ upButtonInfo.name }}</el-button>
    </el-upload>
    <div v-show="upLoading">
      <el-progress ref="progressBar" :percentage="progressPercent" :text-inside="true" :stroke-width="24" status="success" />
    </div>
  </div>
</template>

<script>

// 注：一般不能自动上传服务器，否则无法知道关联表信息，所以必须是在外层调用上传

import CONSTANT from '@/constant'
import fileAPI from '@/api/file'

export default {
  name: 'SimpleUplaod',
  props: {
    value: { type: Array, default: () => [] }, // 父组件 v-model 绑定值
    upButtonInfo: {
      type: Object, default: () => ({
        name: '点击上传',
        tooltip: '只能上传jpg/png文件，且不超过500kb！'
      })
    },
    fileMaxSize: { type: Number, default: CONSTANT.FILE_MAX_SIZE },
    fileAllowedTypes: { type: Array, default: () => [] }, // 支持文件类型
    autoUpload: { type: Boolean, default: true },
    multiple: { type: Boolean, default: true },
    allowMultiFiles: { type: Boolean, default: true },
    updateStatus: { type: Boolean, default: true },
    listType: { type: String, default: 'text' },
  },
  data() {
    return {
      fileList: this.value,
      upLoading: false,
      progressPercent: 0,
      timer: null,
      isManualDelete: true
    }
  },
  watch: {
    value: {
      handler(val) {
        if (val) {
          this.fileList = val          
        }
      }
    }
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
  },
  methods: {
    clearFiles() {
      this.$refs['upload'].clearFiles()
    },
    // 上传之前的回调
    beforeUpload() {
      if (!this.updateStatus) {
        this.$message.warning('当前状态无法上传文件！')
      } else if (this.fileList && (this.fileList.length > 0) && (!this.allowMultiFiles)) {
        this.$message.warning('上传文件已经选择！如需重新选择，请先删除已选择文件！')
      } else {
        this.$refs['upload'].$refs['upload-inner'].handleClick()
      }
    },
    beforeUploadVerify(file) {
      // 首先判断文件大小
      this.isManualDelete = false
      const isLtSize = file.size / 1024 / 1024 < this.fileMaxSize
      if (!isLtSize) {
        this.$message.warning(`上传文件大小不能超过${this.fileMaxSize}MB!`)
        return false
      }
      // 然后判断文件格式是否正确
      const fileName = file.name.lastIndexOf('.')
      const fileNameLength = file.name.length
      const fileFormat = file.name.substring(fileName + 1, fileNameLength)
      if ((this.fileAllowedTypes.length > 0) && (this.fileAllowedTypes.indexOf(fileFormat.toLowerCase()) === -1)) {
        this.$message.warning(this.upButtonInfo.tooltip)
        return false
      }
      // 最后判断是否是重复文件选择
      let i = 0
      this.fileList.forEach(fi => {
        if ((fi.name === file.name) && (fi.size === file.size)) {
          i++
        }
      })
      if (i > 1) {
        this.$message.warning('当前文件已经选择！')
        return false
      }
      this.isManualDelete = true
      return file
    },
    handleChange(file, fileList) {
      this.fileList = fileList
      this.$emit('handle-change', fileList)
    },
    handleBeforeRemove(file, fileList) {
      if (!this.updateStatus) {
        this.$message.warning('当前文件不能被删除！')
        return false
      }
      if (this.isManualDelete) {
        return this.$confirm('删除以后不可恢复，确定删除该文件吗？', '警告', {
          confirmButtonText: '确定',
          cancelButtonText: '取消'
        })
      }
    },
    async handleRemove(file, fileList) {
      if (file.status === 'success') {
        await fileAPI.deleteFile([file.id])
      }
      this.fileList = fileList
    },
    // type: 1 图片；2 文件
    async handleUpload(userId, relId, type) {
      const files = []
      this.fileList.forEach(fi => {
        files.push(fi.raw)
      })
      const params = {
        files: files,
        userId: userId,
        relId: relId,
        type: type
      }
      try {
        this.upLoading = true
        this.timer = setInterval(this.handleProgress, 1000)        
        await fileAPI.upload(params, this.progressPercent)
      } catch (error) {
        this.$message.warning('文件上传失败' + error)
      }
      this.upLoading = false
    },
    handleProgress() {
      this.progressPercent = fileAPI.getProgressPercent()
      if (!this.upLoading) {
        clearInterval(this.timer)
        this.timer = null
      }
    },
    handlePreview(file) {
      if (file.status === 'success') {
        this.$confirm('是否下载该文件？', '提示')
          .then(async _ => {
            this.fullScreenLoading()            
            const content = await fileAPI.downloadFile(file.id)            
            this.downloadFile(content, file.name)
            this.fullScreenLoading().close()
          })
      }
    }
  }
}
</script>
