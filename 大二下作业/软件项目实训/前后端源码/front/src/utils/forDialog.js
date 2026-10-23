
import treeAPI from '@/api/tree'
import listAPI from '@/api/list'
import { Message } from 'element-ui'
import Vue from 'vue'

export default {
  commonSubmitDlg,
  commonSubmitTreeDlg
}

// 保存列表或树结构的表单
async function commonSubmitDlg(formPanel, formData, keyWords, option, isTree, isAudit, userId) {
  var resInfo
  var op = ''
  switch (option) {
    case 'append': op = '新增'
      break
    case 'edit': op = '修改'
      break
    case 'audit': op = '审核'
      break
    case 'submit': op = '提交'
      break
  }
  if (await formPanel.validate()) {
    // const params = filterLastId(formData)
    const params = formData
    if (!isAudit) {
      if (isTree) {
        resInfo = await treeAPI.editOneNode(keyWords, params)
      } else {
        resInfo = await listAPI.editOneNode(keyWords, params)
      }
    } else {
      params.auditUserId = userId
      params.auditTime = new Date()
      resInfo = await listAPI.editOneNode(keyWords, params)
    }
    if (resInfo.message === 'successful') {
      Message.success(`${op}成功！`)
    } else {
      Message.warning(resInfo.message)
    }
  }
  return resInfo
}

function filterLastId(data) {
  // 遍历data，如果data中含有数组并且key不是复数形式就取数组的最后一级id
  Object.keys(data).forEach(key => {
    if (data[key] instanceof Array) {
      // 从单词的最后一个字母是否为 s 判断该单词是否为复数（后端必须规范命名）
      // isArrayOfIds 判断是否是 id 的集合（树结构需要排除childList）
      const isArrayOfIds = !(key.substring(key.length - 1) === 's') && (key !== 'childList')
      if (isArrayOfIds) {
        const length = data[key].length
        data[key] = data[key][length - 1]
      }
    }
  })
  return data
}

// 保存树结构的表单
function commonSubmitTreeDlg(form, formData, keyWords, dialog, saveType = '') {
  // 将新增后的结点信息传回
  var resInfo = {
    successFlag: false,
    nodeInfo: {}
  }
  return new Promise(resolve => {
    form.validate(async valid => {
      if (valid) {
        try {
          const option = dialog.option === 'append' ? '新增' : '修改'
          const params = filterLastId(formData)
          var res = await treeAPI.editOneNode(keyWords, params)
          if (saveType === 'continue') {
            form.resetFields()
            resolve()
          } else {
            form.id = res.data.nodeInfo.id
            resInfo.successFlag = true
            resInfo.nodeInfo = res.data.nodeInfo
            dialog.show = false
          }
          Message.success(`${option}成功！`)
          resolve(resInfo)
        } catch (error) {
          error
        }
      }
    })
  })
}

Vue.directive('dialogDrag', {
  bind(el, binding, vnode, oldVnode) {
    const dialogHeaderEl = el.querySelector('.el-dialog__header')
    const dragDom = el.querySelector('.el-dialog')
    dialogHeaderEl.style.cursor = 'move'
    // 获取原有属性 ie dom元素.currentStyle 火狐谷歌 window.getComputedStyle(dom元素, null);
    const sty = dragDom.currentStyle || window.getComputedStyle(dragDom, null)
    dialogHeaderEl.onmousedown = (e) => {
    // 鼠标按下，计算当前元素距离可视区的距离
      const disX = e.clientX - dialogHeaderEl.offsetLeft
      const disY = e.clientY - dialogHeaderEl.offsetTop
      // 获取到的值带px 正则匹配替换
      let styL, styT
      // 注意在ie中 第一次获取到的值为组件自带50% 移动之后赋值为px
      if (sty.left.includes('%')) {
        styL = +document.body.clientWidth * (+sty.left.replace(/\%/g, '') / 100)
        styT = +document.body.clientHeight * (+sty.top.replace(/\%/g, '') / 100)
      } else {
        styL = +sty.left.replace(/\px/g, '')
        styT = +sty.top.replace(/\px/g, '')
      }

      document.onmousemove = function(e) {
        // 通过事件委托，计算移动的距离
        const l = e.clientX - disX
        const t = e.clientY - disY
        // 移动当前元素
        dragDom.style.left = `${l + styL}px`
        dragDom.style.top = `${t + styT}px`
        // 将此时的位置传出去
        // binding.value({x:e.pageX,y:e.pageY})
      }

      document.onmouseup = function(e) {
        document.onmousemove = null
        document.onmouseup = null
      }
    }
  }
})
