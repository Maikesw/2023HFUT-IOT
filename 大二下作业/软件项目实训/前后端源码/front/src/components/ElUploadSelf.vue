<template>
  <div>
    <el-upload ref="upload" action :class="{hide:fileList.length>=limit}" list-type="picture-card" :file-list="fileList" :drag="drag" :multiple="multiple" :limit="limit" :http-request="()=>{}" :on-change="handleChange" :before-upload="beforeUpload" :auto-upload="false" :on-preview="handlePictureCardPreview" :on-remove="handleRemove">
      <div v-if="drag">
        <i class="el-icon-upload" />
        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
      </div>
      <div v-else class="add-tip">
        <p :class="{'no-limit': !showLimit}" style="font-size: 20px">+</p>
        <p v-if="showLimit">{{ fileList.length + 1 }}/{{ limit }}</p>
      </div>
      <slot slot="tip" name="tip" />
    </el-upload>
    <el-dialog class="image-dialog" :visible.sync="dialogVisible" append-to-body>
      <img width="100%" :src="dialogImageUrl" alt="" />
    </el-dialog>
  </div>
</template>

<script>
export default {
  name: 'Uplaod',
  // 默认情况下，一个组件上的 v-model 会把 value 用作 prop 且把 input 用作 event
  model: {
    prop: 'value', // 注意，是prop，不带s
    event: 'input'
  },
  props: {
    value: { type: Array, default: () => [] }, // 父组件 v-model 绑定值
    limit: { type: Number, default: 1 }, // 文件上传数量限制
    listType: { type: String, default: 'text' }, // 陈列方式，详见 Element UI
    drag: { type: Boolean, default: false }, // 是否支持拖拽
    multiple: { type: Boolean, default: false }, // 是否支持多选
    fileTypes: { type: Array, required: true }, // 支持文件类型
    showLimit: { type: Boolean, default: false } // 是否显示限制数字
  },
  data() {
    return {
      fileList: this.value,
      // 上传文件的数据参数
      dataObj: {},
      dialogVisible: false,
      dialogImageUrl: ''
    }
  },
  // created() {
  //   console.log(this.fileList, 'fileList')
  // },
  methods: {
    handleChange(file, fileList) {
      this.fileList = fileList
      this.$emit('input', fileList)
      this.$emit('change', file, fileList)
    },
    // 删除文件触发事件
    handleRemove(file, fileList) {
      this.fileList = fileList
      this.$emit('input', fileList)
      this.$emit('remove', file, fileList)
    },
    // // 限制文件数量
    // handleExceed(files, fileList) {
    //   this.$message.warning(
    //     `当前限制选择 ${this.limit} 个文件，本次选择了 ${files.length} 个文件，共选择了 ${files.length +
    //     fileList.length} 个文件`
    //   )
    // },
    // 上传之前的回调
    beforeUpload(file) {
      const isLtSize = file.size / 1024 / 1024 < this.CONSTANT.IMAGE_MAX_SIZE
      if (!isLtSize) {
        this.$message.warning(`上传图片大小不能超过 ${this.CONSTANT.IMAGE_MAX_SIZE} MB!`)
        return false
      }
      const fileName = file.name.lastIndexOf('.')
      const fileNameLength = file.name.length
      const fileFormat = file.name.substring(fileName + 1, fileNameLength)
      if (this.fileTypes.indexOf(fileFormat) !== -1) {
        return file
      } else {
        this.$message.warning('上传文件只能是图片格式!')
        return false
      }
    },
    handlePictureCardPreview(file) {
      console.log(file, 'file')
      this.dialogImageUrl = file.url
      this.dialogVisible = true
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep .el-upload-list__item,
::v-deep .el-upload--picture-card {
  height: 60px;
  width: 60px;
  line-height: 60px;
}
.hide {
  ::v-deep .el-upload--picture-card {
    display: none;
  }
}
.add-tip {
  margin: 10px auto;
  p {
    margin: 0 auto;
    line-height: 20px;
  }
  .no-limit {
    margin-top: 20px;
  }
}

::v-deep .el-icon-upload-success,
::v-deep .el-icon-check {
  position: absolute;
  right: 15px;
}

.image-dialog ::v-deep .el-dialog__body {
  border-top: 0 !important;
}
</style>
