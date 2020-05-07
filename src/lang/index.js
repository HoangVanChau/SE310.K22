import Vue from 'vue'
import VueI18n from 'vue-i18n'
import Cookies from 'js-cookie'
import elementEnLocale from 'element-ui/lib/locale/lang/en' // element-ui lang
import elementZhLocale from 'element-ui/lib/locale/lang/zh-CN'// element-ui lang
import elementViLocale from 'element-ui/lib/locale/lang/vi'// element-ui lang
import enLocale from './en'
import zhLocale from './zh'
import viLocale from './vi'

Vue.use(VueI18n)

const messages = {
  en: {
    ...enLocale,
    ...elementEnLocale
  },
  zh: {
    ...zhLocale,
    ...elementZhLocale
  },
  vi: {
    ...viLocale,
    ...elementViLocale
  }
}

const i18n = new VueI18n({
  // set locale
  // options: en or zh
  locale: Cookies.get('language') || 'vi',
  // set locale messages
  messages
})

export default i18n
