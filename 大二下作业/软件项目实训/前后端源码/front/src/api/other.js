import request from '@/utils/request'
import { getEncryptKeyWord } from '@/utils/rsaEncrypt'

export default {
  getKey,
  getWholeKey,
  applyUniAdmin,
  createContestFoundInfo,
  applyRegisterTeam,
  recommendTeams,
  allocateAllExperts,
  calculateAllContestScore,
  calculatePromotion,
  allocateAllExpertswithGroup,
  createCertificate,
  autoInitText
}

function getKey() {
  return request({
    url: '/common/getKey',
    method: 'post'
  })
}

function getWholeKey(key, value) {
  value = value > 9 ? value : '0' + value
  return key + value + key.split('').reverse().join('')
}

// 校级管理员申请
function applyUniAdmin(node) {
  return request({
    url: '/apply/applyUniAdmin',
    method: 'post',
    data: {
      node: JSON.stringify(node)
    }
  })
}

// 竞赛创建
function createContestFoundInfo(node) {
  return request({
    url: '/ContestFlow/createFoundInfo',
    method: 'post',
    data: {
      node: JSON.stringify(node)
    }
  })
}
// 报名队伍创建/修改保存
function applyRegisterTeam(node) {
  return request({
    url: '/apply/applyRegisterTeam',
    method: 'post',
    data: {
      node: JSON.stringify(node)
    }
  })
}
// 校赛/初赛推荐
function recommendTeams(userId, node) {
  return request({
    url: '/apply/recommendTeams',
    method: 'post',
    data: {
      userId: userId,
      node: JSON.stringify(node)
    }
  })
}
// 专家分配/重新分配
function allocateAllExperts(experts, contest, teamStatus) {
  return request({
    url: '/ContestFlow/allocateAllExperts',
    method: 'post',
    data: {
      experts: experts,
      contest: contest,
      teamStatus: teamStatus
    }
  })
}

// 专家分配/重新分配
function allocateAllExpertswithGroup(contest, groups, teamStatus) {
  return request({
    url: '/ContestFlow/allocateAllExpertswithGroup',
    method: 'post',
    data: {
      contest: contest,
      groups: groups,
      teamStatus: teamStatus
    }
  })
}

// 计算/重算总分
function calculateAllContestScore(contestInfoId, flowTypeId) {
  return request({
    url: '/ContestFlow/calculateAllContestScore',
    method: 'post',
    data: { contestInfoId, flowTypeId }
  })
}

// 计算/重算晋级情况
function calculatePromotion(data) {
  return request({
    url: '/ContestFlow/calculatePromotion',
    method: 'post',
    data: { data }
  })
}

// 生成证书
async function createCertificate({ keyWords, pageInfo, treeInfo, searchKey, sort, reg, andor }) {
  keyWords = await getEncryptKeyWord(keyWords)
  return request({
    url: '/ContestFlow/createCertificate',
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
/**
 * 下载中心，附件生成至富文本框
 * @param {*} id 栏目内容id
 * @returns
 */
function autoInitText(id) {
  return request({
    url: '/channel/auto-init-text',
    method: 'post',
    data: { id }
  })
}
