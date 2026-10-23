// import 和 module.exports 不能混用在.vue的文件里面
module.exports = {
  /**
   * @description 网站标题
   */
  title: '安徽省高等学校计算机教育研究会竞赛管理系统',

  title2: '账户登录',

  /**
   * @description 是否显示 tagsView
   */
  tagsView: true,

  /**
   * @type {boolean} true | false
   * @description Whether fix the header
   */

  /**
   * @type {boolean} true | false
   * @description Whether show the logo in sideBar
   */

  /**
   * @description 固定头部
   */
  fixedHeader: false,

  /**
   * @description 记住密码状态下的token在Cookie中存储的天数，默认7天
   */
  tokenCookieExpires: 7,

  /**
   * @description 记住密码状态下的密码在Cookie中存储的天数，默认7天
   */
  passCookieExpires: 7,

  /**
   * @description 是否只保持一个子菜单的展开
   */
  uniqueOpened: true,

  /**
   * @description token key
   */
  TokenKey: 'AXT-TOKEN',

  /**
   * @description Account key 记住密码
   */

  AccountKey: 'AXT-ACCOUNTKEY',
  /**
   * @description 请求超时时间，毫秒（默认2分钟）
   */

  timeout: 1200000,
  /**
   * @description 是否显示logo
   */

  sideBarLogo: true,
  /**
   * 是否显示设置的底部信息
   */

  showFooter: true,
  /**
   * 底部文字，支持html语法
   */

  footerTxt: '測試',
  /**
   *
   */
  prefixApi: 'api'
}
