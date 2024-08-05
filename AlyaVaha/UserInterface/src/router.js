import auth from './auth'
import store from '@/store'
import { createRouter, createWebHistory } from 'vue-router'

import Home from './views/home-page.vue'
import Materialy from './views/materialy-page.vue'
import Zasobniky from './views/zasobniky-page.vue'
import Uzivatelia from './views/uzivatelia-page.vue'
import Navazovania from './views/navazovania-page.vue'
import Statistiky from './views/statistiky-page.vue'
import Profile from './views/profile-page.vue'
import Tasks from './views/tasks-page.vue'
import Settings from './views/nastavenia-page.vue'
import defaultLayout from './layouts/side-nav-outer-toolbar.vue'
import simpleLayout from './layouts/single-card.vue'

function loadView(view) {
  return () => import(/* webpackChunkName: "login" */ `./views/${view}.vue`)
}

const router = new createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Home
    },
    {
      path: '/navazovania',
      name: 'navazovania',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Navazovania
    },
    {
      path: '/statistiky',
      name: 'statistiky',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Statistiky
    },
    {
      path: '/uzivatelia',
      name: 'uzivatelia',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Uzivatelia
    },
    {
      path: '/materialy',
      name: 'materialy',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Materialy
    },
    {
      path: '/zasobniky',
      name: 'zasobniky',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Zasobniky
    },
    {
      path: '/profile',
      name: 'profile',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Profile
    },
    {
      path: '/tasks',
      name: 'tasks',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Tasks
    },
    {
      path: '/settings',
      name: 'settings',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Settings
    },
    {
      path: '/login-form',
      name: 'login-form',
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: 'Prihlásenie'
      },
      component: loadView('login-form')
    },
    {
      path: '/reset-password',
      name: 'reset-password',
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: 'Reset Password',
        description:
          'Please enter the email address that you used to register, and we will send you a link to reset your password via Email.'
      },
      component: loadView('reset-password-form')
    },
    {
      path: '/create-account',
      name: 'create-account',
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: 'Sign Up'
      },
      component: loadView('create-account-form')
    },
    {
      path: '/change-password/:recoveryCode',
      name: 'change-password',
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: 'Change Password'
      },
      component: loadView('change-password-form')
    },
    {
      path: '/recovery',
      redirect: '/'
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: '/'
    }
  ]
})

const loggedInLogic = (to, from, next) => {
  if (to.name === 'login-form' && auth.loggedIn()) {
    next({ name: 'home' })
  }

  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (!auth.loggedIn()) {
      next({
        name: 'login-form',
        query: { redirect: to.fullPath }
      })
    } else {
      next()
    }
  } else {
    next()
  }
}

router.beforeEach((to, from, next) => {
  if (store.isUserLoggedIn === null) {
    return
  }
  loggedInLogic(to, from, next)
})

export default router
