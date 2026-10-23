import request from '@/utils/request'

// 导入
function importInfo(keyWord, file) {
  // const formData = new FormData()
  // formData.append('file', file)
  // return request({
  //   url: `/importAndExport/importInfo?keyWord=${keyWord}`,
  //   method: 'post',
  //   data: formData
  // })
}

// 导出
function exportInfo(keyWords, nodes, allTableColumns, searchWords) {
  return request({
    url: `/importAndExport/exportInfo`,
    method: 'post',
    data: {
      keyWords,
      nodes,
      allTableColumns,
      searchWords
    },
    responseType: 'blob'
  })
}

export default {
  importInfo,
  exportInfo
}
