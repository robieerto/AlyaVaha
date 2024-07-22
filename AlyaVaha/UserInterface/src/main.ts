import 'devextreme/dist/css/dx.common.css'
import './themes/generated/theme.base.css'
import './themes/generated/theme.additional.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap'
import './styles/main.css'

import themes from 'devextreme/ui/themes'
import { locale } from 'devextreme/localization'
import { localizeMessages } from '@/devextremeMessages'

import { createApp } from 'vue'

import App from './App.vue'
import appInfo from './app-info'
import router from './router'

locale('sk')
localizeMessages('sk')

// block reload
document.addEventListener('keydown', (e) => {
  e = e || window.event
  if (e.keyCode == 116) {
    e.preventDefault()
  }
})

themes.initialized(() => {
  const app = createApp(App)
  // app.use(createPinia())
  app.use(router)
  app.config.globalProperties.$appInfo = appInfo
  app.mount('#app')
})
