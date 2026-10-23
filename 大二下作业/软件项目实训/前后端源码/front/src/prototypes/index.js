// 全局挂载
import Vue from 'vue'
import { Loading } from 'element-ui'

// 全局指令式 loading 加载
Vue.prototype.fullScreenLoading = () => {
  return Loading.service({
    lock: true,
    fullscreen: true,
    text: '数据处理中，请稍后……',
    spinner: 'el-icon-loading',
    background: 'rgba(0, 0, 0, 0.7)'
  })
}

// 全局文件下载
Vue.prototype.downloadFile = (content, fileName) => {
  // 字符内容转变成blob地址
  const blob = new Blob([content])
  if ('download' in document.createElement('a')) { // 对象的属性
    // 创建隐藏的可下载链接
    var eleLink = document.createElement('a')
    eleLink.download = fileName
    eleLink.style.display = 'none'

    var URL = window.URL || window.webkitURL
    eleLink.href = URL.createObjectURL(blob)
    // 触发点击
    document.body.appendChild(eleLink)
    eleLink.click()
    // 然后移除
    document.body.removeChild(eleLink)
  } else {
    // IE10+下载
    navigator.msSaveBlob(blob, fileName)
  }
}

// 替换指定字符串前后指定字符
// eslint-disable-next-line no-extend-native
String.prototype.trim = function(char, type) {
  if (char) {
    if (type === 'left') {
      return this.replace(new RegExp('^\\' + char + '+', 'g'), '')
    } else if (type === 'right') {
      return this.replace(new RegExp('\\' + char + '+$', 'g'), '')
    }
    return this.replace(new RegExp('^\\' + char + '+|\\' + char + '+$', 'g'), '')
  }
  return this.replace(/^\s+|\s+$/g, '')
}

import CONSTANT from '@/constant'
Vue.prototype.CONSTANT = CONSTANT

import _ from 'lodash'
Vue.prototype._ = _
