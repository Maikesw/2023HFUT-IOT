<script>
export default {
  name: 'MenuItem',
  functional: true,
  props: { icon: { type: String, default: '' },
    title: { type: String, default: '' }
  },
  render(h, context) {
    const { icon, title } = context.props
    const vnodes = []
    if (icon) {
      // vnodes.push(<svg-icon icon-class={icon}/>)
      const elHtml = h('svg-icon', {
        attrs: { 'icon-class': icon }
      })
      vnodes.push(elHtml)
    }

    if (title) {
      // vnodes.push(<span slot='title'>{(title)}</span>)
      const elHtml = h('span', {
        attrs: { slot: title }
      }, title)
      vnodes.push(elHtml)
    }
    return vnodes
  }
}
</script>
