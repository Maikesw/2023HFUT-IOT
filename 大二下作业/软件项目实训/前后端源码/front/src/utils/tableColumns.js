// 几种最常用的tableColumns
// id, showName, theOrder, tableColumnName 必选
// firstVisiable：默认true； sortable：默认false； width：默认200
export function getTableColumns(type) {
  switch (type) {
    case 1:
      return [ // 流程状态下带审核
        { id: 1, showName: '学校名称', theOrder: 1, tableColumnName: 'schoolName', firstVisible: false, sortable: true },
        { id: 2, showName: '竞赛名称', theOrder: 2, tableColumnName: 'contestName', sortable: true },
        { id: 3, showName: '组别项目', theOrder: 3, tableColumnName: 'smallContestTypeName', firstVisible: false, sortable: true },
        { id: 4, showName: '队伍编码', theOrder: 4, tableColumnName: 'code', sortable: true },
        { id: 5, showName: '队伍名称', theOrder: 5, tableColumnName: 'name', sortable: true },
        { id: 6, showName: '创建人', theOrder: 6, tableColumnName: 'createUserName', firstVisible: false, sortable: true },
        { id: 7, showName: '创建时间', theOrder: 7, tableColumnName: 'createTime', sortable: true },
        { id: 8, showName: '教练', theOrder: 8, tableColumnName: 'allTeacherNames', sortable: true },
        { id: 9, showName: '队员', theOrder: 9, tableColumnName: 'allStudentNames', sortable: true },
        { id: 10, showName: '当前状态', theOrder: 10, tableColumnName: 'isAudit', sortable: true },
        { id: 11, showName: '审核理由', theOrder: 11, tableColumnName: 'reason', sortable: true }
      ]
    case 2:
      return [ // 流程状态下带分数
        { id: 1, showName: '学校名称', theOrder: 1, tableColumnName: 'schoolName', sortable: true },
        { id: 2, showName: '组别项目', theOrder: 2, tableColumnName: 'smallContestTypeName', sortable: true },
        { id: 3, showName: '队伍编码', theOrder: 3, tableColumnName: 'code', sortable: true },
        { id: 4, showName: '队伍名称', theOrder: 4, tableColumnName: 'name', sortable: true },
        { id: 5, showName: '教练', theOrder: 5, tableColumnName: 'allTeacherNames', sortable: true },
        { id: 6, showName: '队员', theOrder: 6, tableColumnName: 'allStudentNames', sortable: true },
        { id: 7, showName: '所得分数', theOrder: 7, tableColumnName: 'score', sortable: true },
        { id: 8, showName: '结果', theOrder: 8, tableColumnName: 'groupResult', sortable: true }
      ]
    case 3:
      return [ // 流程状态下带学校和分数
        { id: 1, showName: '学校名称', theOrder: 1, tableColumnName: 'schoolName', firstVisible: false, sortable: true },
        { id: 2, showName: '竞赛名称', theOrder: 2, tableColumnName: 'contestName', firstVisible: false, sortable: true },
        { id: 3, showName: '组别项目', theOrder: 3, tableColumnName: 'smallContestTypeName', firstVisible: false, sortable: true },
        { id: 4, showName: '队伍编码', theOrder: 4, tableColumnName: 'code', sortable: true },
        { id: 5, showName: '队伍名称', theOrder: 5, tableColumnName: 'name', sortable: true },
        { id: 6, showName: '教练', theOrder: 6, tableColumnName: 'allTeacherNames', sortable: true },
        { id: 7, showName: '队员', theOrder: 7, tableColumnName: 'allStudentNames', sortable: true },
        { id: 8, showName: '作品名称', theOrder: 8, tableColumnName: 'workName', firstVisible: false, sortable: true },
        { id: 9, showName: '决赛所得分数', theOrder: 9, tableColumnName: 'score', sortable: true },
        { id: 10, showName: '决赛修正得分', theOrder: 10, tableColumnName: 'arrangeScore', sortable: true },
        { id: 11, showName: '最终得分', theOrder: 11, tableColumnName: 'allScore', sortable: true },
        { id: 12, showName: '结果', theOrder: 12, tableColumnName: 'groupResult', sortable: true }
      ]
    case 4:
      return [ // 流程状态下带作品信息和摘要
        { id: 1, showName: '竞赛名称', theOrder: 1, tableColumnName: 'contestName', firstVisible: false, sortable: true },
        { id: 2, showName: '组别项目', theOrder: 2, tableColumnName: 'smallContestTypeName', firstVisible: false, sortable: true },
        { id: 3, showName: '队伍编码', theOrder: 3, tableColumnName: 'code', sortable: true },
        { id: 4, showName: '队伍名称', theOrder: 4, tableColumnName: 'name', sortable: true },
        { id: 5, showName: '教练', theOrder: 5, tableColumnName: 'allTeacherNames', sortable: true },
        { id: 6, showName: '队员', theOrder: 6, tableColumnName: 'allStudentNames', sortable: true },
        { id: 7, showName: '作品名称', theOrder: 7, tableColumnName: 'workName', sortable: true },
        { id: 8, showName: '摘要', theOrder: 8, tableColumnName: 'remarks', sortable: true }
      ]
    case 5:
      return [ // 校级管理员申请审核
        { id: 1, showName: '所在单位', theOrder: 1, tableColumnName: 'departmentName', sortable: true, width: 250 },
        { id: 2, showName: '申请人', theOrder: 2, tableColumnName: 'userName', sortable: true, width: 150 },
        { id: 3, showName: '手机号', theOrder: 3, tableColumnName: 'userPhone', firstVisible: false, sortable: true, width: 150 },
        { id: 4, showName: '电子邮箱', theOrder: 4, tableColumnName: 'userEMail', firstVisible: false, sortable: true, width: 150 },
        { id: 5, showName: '申请时间', theOrder: 5, tableColumnName: 'createTime', sortable: true, width: 150 },
        { id: 6, showName: '申请竞赛类型', theOrder: 6, tableColumnName: 'contestTypeName', sortable: true },
        { id: 7, showName: '当前状态', theOrder: 7, tableColumnName: 'isAudit', sortable: true, width: 120 },
        { id: 8, showName: '审核理由', theOrder: 8, tableColumnName: 'reason' }
      ]
    case 6:
      return [ // 专家选择
        { id: 1, showName: '所在单位', theOrder: 1, tableColumnName: 'departmentName' },
        { id: 2, showName: '专家姓名', theOrder: 2, tableColumnName: 'name', sortable: true },
        { id: 3, showName: '联系方式', theOrder: 3, tableColumnName: 'phone', sortable: true }
      ]
    case 7:
      return [ // 专家分配
        { id: 1, showName: '竞赛名称', theOrder: 1, tableColumnName: 'contestName', firstVisible: false },
        { id: 2, showName: '专家单位', theOrder: 2, tableColumnName: 'expertDepartmentName', sortable: true },
        { id: 3, showName: '专家姓名', theOrder: 3, tableColumnName: 'expertName', sortable: true },
        { id: 4, showName: '队伍单位', theOrder: 4, tableColumnName: 'registerGroupSchoolName', sortable: true },
        { id: 5, showName: '队伍编号', theOrder: 5, tableColumnName: 'registerGroupCode', sortable: true },
        { id: 6, showName: '队伍名称', theOrder: 6, tableColumnName: 'registerGroupName', firstVisible: false, sortable: true },
        { id: 7, showName: '作品名称', theOrder: 7, tableColumnName: 'workName', sortable: true },
        { id: 8, showName: '教练', theOrder: 8, tableColumnName: 'registerGroupTeachers', sortable: true },
        { id: 9, showName: '队员', theOrder: 9, tableColumnName: 'registerGroupTeamers', sortable: true }
      ]
    case 8:
      return [ // 网上评审
        { id: 1, showName: '竞赛名称', theOrder: 1, tableColumnName: 'contestName', firstVisible: true },
        { id: 2, showName: '队伍编号', theOrder: 2, tableColumnName: 'registerGroupCode', sortable: true },
        { id: 3, showName: '作品名称', theOrder: 3, tableColumnName: 'workName', sortable: true },
        { id: 4, showName: '所得分数', theOrder: 4, tableColumnName: 'score', sortable: true }
      ]
    case 9:
      return [ // 竞赛查看（竞赛管理员）
        { id: 1, showName: '学校名称', theOrder: 1, tableColumnName: 'schoolName', firstVisible: false, sortable: true },
        { id: 2, showName: '竞赛名称', theOrder: 2, tableColumnName: 'contestName', sortable: true },
        { id: 3, showName: '组别项目', theOrder: 3, tableColumnName: 'smallContestTypeName', firstVisible: false, sortable: true },
        { id: 4, showName: '队伍编码', theOrder: 4, tableColumnName: 'code', sortable: true },
        { id: 5, showName: '队伍名称', theOrder: 5, tableColumnName: 'name', sortable: true },
        { id: 6, showName: '创建人', theOrder: 6, tableColumnName: 'createUserName', firstVisible: false, sortable: true },
        { id: 7, showName: '创建时间', theOrder: 7, tableColumnName: 'createTime', sortable: true },
        { id: 8, showName: '更新时间', theOrder: 8, tableColumnName: 'updateTime', firstVisible: false, sortable: true },
        { id: 9, showName: '教练', theOrder: 9, tableColumnName: 'allTeacherNames', sortable: true },
        { id: 10, showName: '队员', theOrder: 10, tableColumnName: 'allStudentNames', sortable: true },
        { id: 11, showName: '教练手机号', theOrder: 11, tableColumnName: 'allTeacherPhones', firstVisible: false, sortable: true },
        { id: 12, showName: '队员手机号', theOrder: 12, tableColumnName: 'allStudentPhones', firstVisible: false, sortable: true },
        { id: 13, showName: '级别', theOrder: 13, tableColumnName: 'status', sortable: true },
        { id: 14, showName: '状态', theOrder: 14, tableColumnName: 'isAudit', sortable: true },
        { id: 15, showName: '作品名称', theOrder: 15, tableColumnName: 'workName', firstVisible: false, sortable: true },
        { id: 16, showName: '所得分数', theOrder: 16, tableColumnName: 'score', firstVisible: false, sortable: true },
        { id: 17, showName: '加权分数', theOrder: 17, tableColumnName: 'arrangeScore', firstVisible: false, sortable: true },
        { id: 18, showName: '最终得分', theOrder: 18, tableColumnName: 'allScore', sortable: true },
        { id: 19, showName: '结果', theOrder: 19, tableColumnName: 'groupResult', sortable: true }
      ]
    case 10:
      return [ // 竞赛查看(校级管理员以下)
        { id: 1, showName: '学校名称', theOrder: 1, tableColumnName: 'schoolName', firstVisible: false, sortable: true },
        { id: 2, showName: '竞赛名称', theOrder: 2, tableColumnName: 'contestName', sortable: true },
        { id: 3, showName: '组别项目', theOrder: 3, tableColumnName: 'smallContestTypeName', firstVisible: false, sortable: true },
        { id: 4, showName: '队伍编码', theOrder: 4, tableColumnName: 'code', sortable: true },
        { id: 5, showName: '队伍名称', theOrder: 5, tableColumnName: 'name', sortable: true },
        { id: 6, showName: '创建人', theOrder: 6, tableColumnName: 'createUserName', firstVisible: false, sortable: true },
        { id: 7, showName: '创建时间', theOrder: 7, tableColumnName: 'createTime', sortable: true },
        { id: 8, showName: '教练', theOrder: 8, tableColumnName: 'allTeacherNames', sortable: true },
        { id: 9, showName: '队员', theOrder: 9, tableColumnName: 'allStudentNames', sortable: true },
        { id: 10, showName: '级别', theOrder: 10, tableColumnName: 'status', sortable: true },
        { id: 11, showName: '状态', theOrder: 11, tableColumnName: 'isAudit', sortable: true },
        { id: 12, showName: '作品名称', theOrder: 12, tableColumnName: 'workName', firstVisible: false, sortable: true },
        { id: 13, showName: '获得奖项(是否通过)', theOrder: 13, tableColumnName: 'groupResult', sortable: true }
        // { id: 14, showName: '最终得分', theOrder: 14, tableColumnName: 'allScore', sortable: true }
      ]
    case 11:
      return [ // 现场专家分配分组
        { id: 1, showName: '竞赛名称', theOrder: 1, tableColumnName: 'contestName', firstVisible: false, sortable: true },
        { id: 2, showName: '分组编码', theOrder: 2, tableColumnName: 'code', width: 150 },
        { id: 3, showName: '分组名称', theOrder: 3, tableColumnName: 'name', width: 250 },
        { id: 4, showName: '专家姓名', theOrder: 4, tableColumnName: 'allExpertNames', width: 250 },
        { id: 5, showName: '其他说明', theOrder: 5, tableColumnName: 'remarks' }
      ]
    default:
      return [
        { id: 1, showName: '编码', theOrder: 1, tableColumnName: 'code', width: 150 },
        { id: 2, showName: '名称', theOrder: 2, tableColumnName: 'name', width: 250 },
        { id: 3, showName: '备注', theOrder: 3, tableColumnName: 'remarks' }
      ]
  }
}
