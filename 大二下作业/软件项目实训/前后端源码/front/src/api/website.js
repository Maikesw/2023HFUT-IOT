import request from '@/utils/request'

export default {
  column,
  columnContent,
  homePage,
  readNum,
  search
}

// 获取栏目类别
function column() {
  return request({
    url: '/website/column',
    method: 'get'
  })
}

// 新闻中心
function columnContent(data) {
  return request({
    url: '/website/columnContent',
    method: 'post',
    data
  })
}

// 首页
function homePage() {
  return request({
    url: '/website/homePage',
    method: 'get'
  })
}

// 增加阅读数
function readNum(id) {
  return request({
    url: `website/readNum?id=${id}`,
    method: 'get'
  })
}

// 首页搜索
function search(word) {
  return request({
    url: `website/search?word=${word}`,
    method: 'get'
  })
}
