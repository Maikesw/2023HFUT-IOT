
import axios from 'axios'
import { Message, MessageBox } from 'element-ui'
import store from '@/store'
// 创建aaxios实例
const service = axios.create({
  // baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  baseURL: '/api',
  withCredentials: true, // send cookies when cross-domain requests
  timeout: 120 * 1000 // request timeout
})

// request interceptor 这个部分保留，当后端系统需要传递唯一凭证的token时使用
service.interceptors.request.use(
  // config => {
  //   // do something before request is sent
  //   if (store.getters.token) {
  //     // let each request carry token
  //     // ['X-Token'] is a custom headers key
  //     // please modify it according to the actual situation
  //     config.headers['Authorization'] = getToken()
  //   }
  //   return config
  // },
  // error => {
  //   // do something with request error
  //   console.log(error) // for debug
  //   return Promise.reject(error)
  // }
)

// response interceptor
service.interceptors.response.use(

  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
  */

  /**
   * Determine the request status by custom status
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    const res = response.data
    if (!res.code) { // 文件下载
      return res
    }

    // if the custom status is not 200, it is judged as an error.
    if (res.status !== 200) {
      Message({
        message: res.message || 'Error',
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(new Error(res.message || 'Error'))
    } else {
      return res
    }
  },
  (error) => {
    const errorData = error?.response?.data
    // 4007：后台的token失效，需要重新获取
    if (errorData.status === 4007) {
      MessageBox.confirm(
        '登录状态已失效，请重新登录',
        '确定退出', {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning',
          showClose: false
        }
      ).then(() => {
        store.dispatch('user/logout').then(() => {
          location.reload() // 为了重新实例化vue-router对象 避免bug
        })
      })
    } else {
      Message({
        message: errorData.message,
        type: 'error',
        duration: 3 * 1000
      })
      return Promise.reject(error)
    }
  })

export default service
