import request from '@/utils/request'
import { getEncryptKeyWord } from '@/utils/rsaEncrypt'
import constant from '@/constant'

export default {
  getAllNodes,
  delOneNode,
  delManyNode,
  changeTwoNodes,
  editOneNode,
  getAllBrotherIndex,
  getAllParentIndex,
  commonSearch
}
// 获得所有树节点
async function getAllNodes({ keyWords, parentId = -1, virtualRootFlag = true, searchKey = '', lazy = false, preName = '', sort }) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataTree/readAllTreeNodes',
    method: 'post',
    data: {
      keyWords,
      parentId,
      virtualRootFlag,
      searchKey,
      lazy,
      preName,
      sort
    }
  })
}
// 交换两个树节点位置
async function changeTwoNodes(keyWords, nodeId, nodeChangeId) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataTree/changeTwoNodes',
    method: 'post',
    data: {
      keyWords: keyWords,
      nodeId: nodeId,
      nodeChangeId: nodeChangeId
    }
  })
}
// 删除一个节点
async function delOneNode(keyWords, node) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataTree/delOneNode',
    method: 'post',
    data: {
      keyWords: keyWords,
      node: JSON.stringify(node)
    }
  })
}
// 删除多个节点
async function delManyNode(keyWords, nodes) {
  keyWords = await getEncryptKeyWord(keyWords)
  console.log(nodes.map(e => e.id).join(constant.SPLIT_OPERATOR.COMMA))
  return request({
    url: '/dataTree/delManyNode',
    method: 'post',
    data: {
      keyWords: keyWords,
      nodes: JSON.stringify(nodes)
      // nodes: getEncryptKeyWord(nodes.map(e => e.id).join(constant.SPLIT_OPERATOR.COMMA))
    }
  })
}

// 编辑一个节点（包括新增）
// 传theOrder且为-1.代表无theOrder字段，其他情况需要对theOrder字段进行赋值处理
async function editOneNode(keyWords, node) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataTree/editOneNode',
    method: 'post',
    data: {
      keyWords: keyWords,
      node: JSON.stringify(node)
    }
  })
}
// 获得所有父节点
async function getAllParentIndex(keyWords, nodeId) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataTree/getAllParentIndex',
    method: 'post',
    data: {
      keyWords: keyWords,
      nodeId: nodeId
    }
  })
}
// 获得当前节点的直接父亲节点
async function getAllBrotherIndex(keyWords, nodeId) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/dataTree/getAllBrotherIndex',
    method: 'post',
    data: {
      keyWords: keyWords,
      nodeId: nodeId
    }
  })
}

// 树结构条件的列表查询
function commonSearch({ treeInfo, listKeyWords, searchKey, regKey, pageInfo, sortJson }) {
  return request({
    url: '/dataTree/commonSearch',
    method: 'post',
    data: {
      treeInfo,
      listKeyWords,
      searchKey,
      regKey,
      pageInfo,
      sortJson
    }
  })
}
