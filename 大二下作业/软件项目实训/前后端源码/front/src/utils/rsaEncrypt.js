import constant from '@/constant'
import otherAPI from '@/api/other'

const CryptoJS = require('crypto-js')
// const data = '888888'
// const srcs = CryptoJS.enc.Utf8.parse(data)
const key = CryptoJS.enc.Utf8.parse(constant.CRYPT)

async function Encrypt(word) {
  const key1 = await otherAPI.getKey()
  const key = CryptoJS.enc.Utf8.parse(otherAPI.getWholeKey(key1.data.key, key1.data.value))
  // console.log(otherAPI.getWholeKey(key1.data.key, key1.data.value))
  var srcs = CryptoJS.enc.Utf8.parse(word)
  var encrypted = CryptoJS.AES.encrypt(srcs, key, { mode: CryptoJS.mode.ECB, padding: CryptoJS.pad.Pkcs7 })

  var value = key1.data.value
  value = value > 9 ? value : '0' + value
  return encrypted.toString() + value
}
function Decrypt(word) {
  var decrypt = CryptoJS.AES.decrypt(word, key, { mode: CryptoJS.mode.ECB, padding: CryptoJS.pad.Pkcs7 })
  return CryptoJS.enc.Utf8.stringify(decrypt).toString()
}

async function getEncryptKeyWord(keyword) {
  // var date = new Date()
  // var minute = date.getMinutes()
  // var second = date.getSeconds()
  // minute = minute > 9 ? minute : '0' + minute
  // second = second > 9 ? second : '0' + second
  // return Encrypt( Math.floor(Math.random() * 10) + keyword + Math.floor(Math.random() * 10))
  // return Encrypt( minute + keyword + second)
  return Encrypt(keyword)
  // console.log(key)
  // console.log(otherAPI.getKey())
  // var key1 = await otherAPI.getKey()
  // const key = CryptoJS.enc.Utf8.parse(key1.data)
  // var srcs = CryptoJS.enc.Utf8.parse(keyword)
  // var encrypted = CryptoJS.AES.encrypt(srcs, key, {mode:CryptoJS.mode.ECB, padding: CryptoJS.pad.Pkcs7})
  // return encrypted.toString()
}

export {
  Decrypt,
  Encrypt,
  getEncryptKeyWord
}
