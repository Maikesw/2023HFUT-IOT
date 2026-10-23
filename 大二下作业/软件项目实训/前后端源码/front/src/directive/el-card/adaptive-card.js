// bind：只调用一次，指令第一次绑定到元素时调用。在这里可以进行一次性的初始化设置。
// inserted：被绑定元素插入父节点时调用 (仅保证父节点存在，但不一定已被插入文档中)。
// unbind：只调用一次，指令与元素解绑时调用。

// 设置表格高度
const doResize = async(el, binding, vnode) => {
  // 获取表格Dom对象
  const {
    componentInstance: $card
  } = await vnode
  // 获取调用传递过来的数据
  const {
    value
  } = binding

  // 获取距底部距离（用于展示页码等信息）
  const bottomOffset = (value && value.bottomOffset) || 19

  const minHeight = 231

  if (!$card) return

  // 计算列表高度并设置
  const totalHeight = window.innerHeight - el.getBoundingClientRect().top - bottomOffset
  const height = totalHeight < minHeight ? minHeight : totalHeight

  $card.$el.style.height = height + 'px'
  $card.$el.style.transition = 'unset'
}

export default {
  // 初始化设置
  bind(el, binding, vnode) {
    // 设置resize监听方法
    el.resizeListener = async() => {
      await doResize(el, binding, vnode)
    }
    // 绑定监听方法到addResizeListener
    // addResizeListener(window.document.body, el.resizeListener)
    window.addEventListener('resize', el.resizeListener)
  },
  update(el, binding, vnode) {
    doResize(el, binding, vnode)
  },
  // 绑定默认高度
  async inserted(el, binding, vnode) {
    await doResize(el, binding, vnode)
  },
  // 销毁时设置
  unbind(el) {
    // 移除resize监听
    // removeResizeListener(el, el.resizeListener)
    window.removeEventListener('resize', el.resizeListener)
  }
}

