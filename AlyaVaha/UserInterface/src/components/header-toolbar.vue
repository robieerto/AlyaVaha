<template>
  <header class="header-component">
    <dx-toolbar class="header-toolbar">
      <dx-item :visible="menuToggleEnabled" location="before" css-class="menu-button">
        <template #default>
          <dx-button icon="menu" styling-mode="text" @click="toggleMenuFunc" />
        </template>
      </dx-item>

      <dx-item location="before" css-class="header-title dx-toolbar-label">
        <div class="alya-logo" />
      </dx-item>

      <dx-item
        v-if="store.zariadenie"
        location="before"
        css-class="header-title dx-toolbar-label px-3"
      >
        <div>{{ store.zariadenie?.NazovZariadenia }}:</div>
      </dx-item>

      <dx-item v-if="title" location="before" css-class="header-title dx-toolbar-label">
        <p
          :class="
            'col fw-bold status contact-status status-' + store.connected?.toString().toLowerCase()
          "
        >
          {{ store.connected ? 'Pripojená' : 'Odpojená' }}
        </p>
      </dx-item>

      <dx-item location="after" locate-in-menu="auto" menu-item-template="menuUserItem">
        <template #default>
          <div>
            <dx-button
              class="user-button authorization"
              :width="210"
              height="100%"
              styling-mode="text"
            >
              <user-panel :username="username" :menu-items="userMenuItems" menu-mode="context" />
            </dx-button>
          </div>
        </template>
      </dx-item>

      <template #menuUserItem>
        <user-panel :username="username" :menu-items="userMenuItems" menu-mode="list" />
      </template>
    </dx-toolbar>
  </header>
</template>

<script>
import DxButton from 'devextreme-vue/button'
import DxToolbar, { DxItem } from 'devextreme-vue/toolbar'
import auth from '../auth'
import { useRouter, useRoute } from 'vue-router'
import { ref } from 'vue'

import UserPanel from './user-panel.vue'

import store, { resetLoggedInZariadenieData } from '@/store'
import { sendCommand } from '@/commandHandler'

export default {
  props: {
    menuToggleEnabled: Boolean,
    title: String,
    toggleMenuFunc: Function,
    logOutFunc: Function
  },
  setup() {
    const router = useRouter()
    const route = useRoute()

    const username = ref('')
    auth.getUser().then((e) => {
      username.value = e.data.Login
    })

    const userMenuItems = [
      // {
      //   text: 'Profil',
      //   icon: 'user',
      //   onClick: onProfileClick
      // },
      {
        text: 'Odhlásiť sa',
        icon: 'login',
        onClick: onLogoutClick
      }
    ]

    function onLogoutClick() {
      sendCommand('Logout')
      resetLoggedInZariadenieData()
      store.isUserLoggedIn = false
      router.push({
        path: '/login-form',
        query: { redirect: route.path }
      })
    }

    function onProfileClick() {
      router.push({
        path: '/profile',
        query: { redirect: route.path }
      })
    }

    return {
      username,
      userMenuItems,
      store
    }
  },
  components: {
    DxButton,
    DxToolbar,
    DxItem,
    UserPanel
  }
}
</script>

<style lang="scss">
@import '../themes/generated/variables.base.scss';
@import '../dx-styles.scss';

.header-component {
  flex: 0 0 auto;
  z-index: 1;
  box-shadow:
    0 1px 3px rgba(0, 0, 0, 0.12),
    0 1px 2px rgba(0, 0, 0, 0.24);

  .dx-toolbar .dx-toolbar-item.menu-button > .dx-toolbar-item-content .dx-icon {
    color: $base-accent;
  }
}

.dx-toolbar.header-toolbar .dx-toolbar-items-container .dx-toolbar-after {
  padding: 0 40px;

  .screen-x-small & {
    padding: 0 20px;
  }
}

.dx-toolbar .dx-toolbar-item.dx-toolbar-button.menu-button {
  width: $side-panel-min-width;
  text-align: center;
  padding: 0;
}

.header-title .dx-item-content {
  padding: 0;
  margin: 0;
}

.dx-theme-generic {
  .dx-toolbar {
    padding: 10px 0;
  }

  .user-button > .dx-button-content {
    padding: 3px;
  }
}
</style>

<style lang="scss">
.contact-status {
  @mixin status($status-color) {
    color: $status-color;

    &.dx-texteditor-input.status-editor-input {
      color: $status-color;
    }

    &::before {
      background: $status-color;
    }
  }
  &.status-undefined {
    @include status(#03a9f4);
  }

  &.status-true {
    @include status(#2eb52c);
  }

  &.status-false {
    @include status(#e3000f);
  }
}
</style>

<style scoped lang="scss">
@use 'sass:math';

.contact-status {
  &::before {
    --diameter: 12px;

    content: ' ';
    width: var(--diameter);
    height: var(--diameter);
    border-radius: calc(var(--diameter) / 2);
    margin-right: calc(var(--diameter) / 2);
    display: inline-block;
    align-self: center;
  }

  :deep(&.input) {
    display: block;
    padding: 15px 16px 14px;
  }
}

.alya-logo {
  width: 90px;
  height: 30px;
  background: url('../assets/alya-logo.png') no-repeat;
  background-size: cover;
}
</style>
