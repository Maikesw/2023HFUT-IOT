import request from '@/utils/request'
import { getEncryptKeyWord } from '@/utils/rsaEncrypt'
import constant from '@/constant'

export default {
  editOneNode,
  delOneOrManyNodes,
  changeNodeOrder,
  getSomeRecords
}

// 列表结构的新增/编辑
async function editOneNode(keyWords, node) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataList/editOneNode',
    method: 'post',
    data: {
      keyWords: keyWords,
      node: JSON.stringify(node)
    }
  })
}
// 删除一个或者多个节点
async function delOneOrManyNodes(keyWords, ids) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataList/delOneOrManyNodes',
    method: 'post',
    data: {
      keyWords: keyWords,
      ids: await getEncryptKeyWord(ids.join(constant.SPLIT_OPERATOR.COMMA))
    }
  })
}
// 列表结构的节点上下移
async function changeNodeOrder(keyWords, nodeId, up, moveSearchKeys, moveRegKeys) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataList/changeNodeOrder',
    method: 'post',
    data: {
      keyWords: keyWords,
      nodeId: nodeId,
      up: up,
      moveSearchKeys: moveSearchKeys,
      moveRegKeys: moveRegKeys
    }
  })
}

// 列表结构的查询
async function getSomeRecords({ keyWords, pageInfo, treeInfo, searchKey, sort, reg, andor }) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataList/getSomeRecords',
    method: 'post',
    data: {
      keyWords,
      pageInfo,
      treeInfo,
      searchKey: await getEncryptKeyWord(JSON.stringify(searchKey)),
      sort,
      reg: await getEncryptKeyWord(JSON.stringify(reg)),
      andor
    }
  })
}
