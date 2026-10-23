/** 封装一些常用的、全局调用的js方法 */
// import CONSTANT from '@/constant'
import _ from 'lodash'
// 获取导入/导出文件名及导入文件模板id
export function getTemplateFile(keywords, suffix = '') {
  switch (keywords) {
    case '校级报名表':
      return { name: '校级报名表' + suffix, id: null }
    // case 'TblMenuInfo': // 菜单管理
    //   return { name: '菜单' + suffix, id: null }
    // case 'TblRoleInfo': // 角色管理
    //   return { name: '角色' + suffix, id: null }
    // case 'TblDepartmentInfo': // 部门管理
    //   return { name: '部门' + suffix, id: null }
    // case 'TblJobInfo': // 岗位管理
    //   return { name: '岗位' + suffix, id: null }
    // case 'TblUserInfo': // 平台员工管理
    //   return { name: '员工' + suffix, id: CONSTANT.TEMPLATE.STAFF_ID }
    default:
      return { name: '导出数据' + suffix, id: null }
  }
}

export function convertDateStr(date, simple) {
  if (date == null) {
    date = new Date()
  }
  var year = date.getFullYear()
  var month = date.getMonth() + 1
  var strDate = date.getDate()
  var hour = date.getHours()
  var minute = date.getMinutes()
  var second = date.getSeconds()
  month = month > 9 ? month : '0' + month
  strDate = strDate > 9 ? strDate : '0' + strDate
  hour = hour > 9 ? hour : '0' + hour
  minute = minute > 9 ? minute : '0' + minute
  second = second > 9 ? second : '0' + second
  var newdata = ''
  if (simple) {
    newdata = year + '-' + month + '-' + strDate
  } else {
    newdata = year + '-' + month + '-' + strDate + ' ' + hour + ':' + minute + ':' + second
  }
  return newdata
}

export function compareTime(gettime) {
  var today = new Date() // 获取当前时间
  gettime = gettime.replace(/-/g, '/')
  // 转化成时间戳作比较
  var endTime = new Date(gettime) // 自己的时间
  if (today.getTime() > endTime.getTime()) {
    // 当前时间大于我的时间
    return false
  } else {
    // 当前时间小于我的时间
    return true
  }
}

export function arrangeButton(compareObj, transObj) {
  if (compareObj !== undefined) {
    Object.keys(transObj).forEach(key1 => {
      if (compareObj[key1] !== undefined) {
        Object.keys(transObj[key1]).forEach(key2 => {
          if (compareObj[key1][key2] !== undefined) {
            transObj[key1][key2] = compareObj[key1][key2]
          }
        })
      }
    })
  }
  return transObj
}

// 去除字符串首尾逗号
export function commaTrim(str) {
  return str.replace(/^(,)+|(,)+$/g, '')
}

// 获取数值的类型
// instanceof运算符返回一个布尔值，表示指定对象是否为某个构造函数的实例
// instanceof运算符只能用于对象，不适用原始类型的值
export function getValueType(value) {
  var type = ''
  if (typeof value !== 'object') {
    type = typeof value
  } else {
    if (value instanceof Array) {
      type = 'array'
    } else {
      if (value instanceof Object) {
        type = 'object'
      } else {
        type = 'null'
      }
    }
  }
  return type
}

function _customize(obj, src) {
  var ret
  if ((typeof obj) !== 'undefined') { // 首先看目标，目标有的全上
    ret = _.cloneDeep(obj)
  }
  // 然后看源。源有且只有一个，比较目的有没有，有就上。源有多个，再循环
  if ((typeof src) !== 'undefined') {
    if (src instanceof Array) { // 如果是数组
      if ((typeof obj) === 'undefined') {
        ret = Object.values(_.merge(ret, src))
      }
    } else {
      if (Object.keys(src).length === 1) {
        if ((typeof obj) === 'undefined') {
          ret = _.merge(ret, src)
        }
      } else {
        if ((typeof obj) === 'undefined') {
          if ((typeof src) === 'object') {
            ret = _.merge(ret, src)
          } else {
            ret = src
          }
        } else {
          if ((typeof src) === 'string') {
            ret = _.merge(ret, src)
          } else {
            Object.keys(src).forEach(key => {
              var ob = {}
              _.set(ob, key, _customize(obj[key], src[key]))
              ret = _.merge(ret, ob)
            })
          }
        }
      }
    }
  }
  return ret
}
export function customize(obj, src) {
  var ret = {}
  ret = _.cloneDeep(_customize(obj, src))
  return ret
}

// 将数值重置为初始类型（用于清空表单操作）
export function resetForm(form) {
  const theOrder = form.theOrder === -1
  Object.keys(form).forEach(key => {
    const type = getValueType(form[key])
    if (type === 'number') {
      form[key] = null
    } else if (type === 'string') {
      form[key] = ''
    } else if (type === 'boolean') {
      form[key] = false
    } else if (type === 'undefined') {
      form[key] = undefined
    } else if (type === 'null') {
      form[key] = null
    } else if (type === 'array') {
      form[key] = []
    } else if (type === 'object') {
      form[key] = {}
    }
  })
  if (theOrder) { form.theOrder = -1 }
  return form
}

// 判断对象中的值是否全部为空
export function isObjEmpty(obj) {
  const arr = Object.values(obj)
  const arr_v2 = []
  arr.forEach(item => {
    if (!item) { arr_v2.push(item) }
  })
  const flag = arr_v2.length === arr.length
  return flag
}

// 树结构重命名
export function treeReconfiguration(list) {
  const arr = []
  list.forEach(item => {
    const node = {
      id: item.id,
      label: item.name,
      children: []
    }
    if (item.children.length > 0) {
      const children = treeReconfiguration(item.children)
      node.children = children
    }
    arr.push(node)
  })
  return arr
}
// 清除富文本内所有标签、样式、图片
export function removeHtmlStyle(html) {
  const relStyle = /style\s*?=\s*?([‘"])[\s\S]*?\1/g // 去除样式
  const relTag = /<.+?>/g // 去除标签
  const relClass = /class\s*?=\s*?([‘"])[\s\S]*?\1/g// 清除类名
  const relSpace = /&nbsp;|\\n|\\r|&quot;|&amp;|&lt;|&gt;/g // 清除html转义字符
  let newHtml = ''
  if (html) {
    newHtml = html.replace(relStyle, '')
    newHtml = newHtml.replace(relTag, '')
    newHtml = newHtml.replace(relClass, '')
    newHtml = newHtml.replaceAll(relSpace, '')
  }
  return newHtml
}
