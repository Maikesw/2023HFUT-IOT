import CONSTANT from '@/constant'
import listAPI from '@/api/list'
import store from '@/store'
import _ from 'lodash'

export function getTeamStatus(flowName) {
  switch (flowName) {
    case CONSTANT.CONTEST_FLOW.SCHOOL_REGISTER:
    case CONSTANT.CONTEST_FLOW.SCHOOL_VERIFY:
    case CONSTANT.CONTEST_FLOW.SCHOOL_RESULT:
    case CONSTANT.CONTEST_FLOW.SCHOOL_RECOMMENDATION:
      return CONSTANT.TEAM_STATUS.SCHOOL_CONTEST
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_REGISTER:
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_VERIFY:
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_RESULT:
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_UPLOAD_WORKS:
    case CONSTANT.CONTEST_FLOW.ALLOCATE_PRELIMINARY_EXPERT:
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_REVIEW:
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_OPEN:
      return CONSTANT.TEAM_STATUS.FIRST_CONTEST
      case CONSTANT.CONTEST_FLOW.LAST_REGISTER:
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_RECOMMENDATION:
    case CONSTANT.CONTEST_FLOW.PRELIMINARY_RECOMMEND_VERIFY:
    case CONSTANT.CONTEST_FLOW.LAST_UPLOAD_WORKS:
    case CONSTANT.CONTEST_FLOW.ALLOCATE_LAST_EXPERT:
    case CONSTANT.CONTEST_FLOW.LAST_REVIEW:
    case CONSTANT.CONTEST_FLOW.LAST_OPEN:
    case CONSTANT.CONTEST_FLOW.LAST_RESULT:
    case CONSTANT.CONTEST_FLOW.LAST_ANNOUNCE:
      return CONSTANT.TEAM_STATUS.LAST_CONTEST
  }
}
// 获得当前流程，当前时间段内的竞赛id
export async function getNowFlowContests(flowName) {    
  const res = await listAPI.getSomeRecords({
    keyWords: 'ViewContestFlowDetail',
    searchKey: {
      flowName: flowName,
      startDate: new Date(),
      endDate: new Date()
    },
    reg: {
      startDate: CONSTANT.SEARCH_OPERATOR.LE,
      endDate: CONSTANT.SEARCH_OPERATOR.GE
    }
  })
  var contestIds = '0'  
  if (res.data.content.length > 0) {
    contestIds = res.data.content.map(e => e.contestInfoId).join(CONSTANT.SPLIT_OPERATOR.COMMA)
  }  
  return contestIds
}
// 根据当前流程的名称获得当前流程的id
export async function getFlowTypeId(flowName) {
  const res = await listAPI.getSomeRecords({
    keyWords: 'BaseContestFlowType',
    searchKey: { name: flowName }
  })
  var id = 0
  if (res.data.content.length > 0) {
    id = res.data.content[0].id
  }
  return id
}
// 根据角色获得当前角色能操作的contestTypeId
export async function getRoleContestType(role) {
  const res = await listAPI.getSomeRecords({
    keyWords: 'RelUserContestType',
    searchKey: { userId: store.getters.userInfo.id, roleId: role }
  })
  var ids = '0'
  if (res.data.content.length > 0) {
    ids = res.data.content.map(e => e.typeId).join(CONSTANT.SPLIT_OPERATOR.COMMA)
  }
  return ids
}

export async function setSearchKeyHeader(vueObject) {  
  // console.log(vueObject.flowName)  
  var contestIds = await getNowFlowContests(vueObject.flowName)
  if (contestIds !== '0') {
    _.set(vueObject.initSearchWords.searchKey, 'contestInfoId', contestIds)
    _.set(vueObject.initSearchWords.regKey, 'contestInfoId', CONSTANT.SEARCH_OPERATOR.IN)
    // 不管哪种角色，只能看到当前team状态的队伍
    _.set(vueObject.initSearchWords.searchKey, 'status', vueObject.teamStatus)
    const roleInfo = store.getters.roles
    if (roleInfo.indexOf(CONSTANT.ROLE_TABLE.SUPER_ADMIN) === -1) { // 超级管理员全部放开
      if (roleInfo.indexOf(CONSTANT.ROLE_TABLE.CONTEST_ADMIN) !== -1) { // 竞赛管理员
        const ids = await getRoleContestType(CONSTANT.ROLE_TABLE.CONTEST_ADMIN)
        _.set(vueObject.initSearchWords.searchKey, 'contestTypeId', ids)
        _.set(vueObject.initSearchWords.regKey, 'contestTypeId', CONSTANT.SEARCH_OPERATOR.IN)
      } else if (roleInfo.indexOf(CONSTANT.ROLE_TABLE.UNIVERSITY_ADMIN) !== -1) { // 校级管理员，不可能同时为竞赛管理员
        const ids = await getRoleContestType(CONSTANT.ROLE_TABLE.UNIVERSITY_ADMIN)
        _.set(vueObject.initSearchWords.searchKey, 'contestTypeId', ids)
        _.set(vueObject.initSearchWords.regKey, 'contestTypeId', CONSTANT.SEARCH_OPERATOR.IN)
        _.set(vueObject.initSearchWords.searchKey, 'schoolId', store.getters.userInfo.schoolId)
      }
    }
  } else {
    _.set(vueObject.initSearchWords.searchKey, 'contestInfoId', '0')
  }  
}
