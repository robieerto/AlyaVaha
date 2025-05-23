<template>
  <form class="login-form" @submit.prevent="onSubmit">
    <dx-form :form-data="formData" :disabled="loading">
      <dx-item
        data-field="email"
        editor-type="dxTextBox"
        :editor-options="{ stylingMode: 'filled', placeholder: 'Užívateľské meno' }"
      >
        <dx-required-rule message="Užívateľské meno je povinné" />
        <!-- <dx-email-rule message="Email je nesprávny" /> -->
        <dx-label :visible="false" />
      </dx-item>
      <dx-item
        data-field="password"
        editor-type="dxTextBox"
        :editor-options="{ stylingMode: 'filled', placeholder: 'Heslo', mode: 'password' }"
      >
        <dx-required-rule message="Heslo je povinné" />
        <dx-label :visible="false" />
      </dx-item>
      <dx-item
        data-field="zariadenieId"
        editor-type="dxSelectBox"
        :editor-options="{
          stylingMode: 'filled',
          placeholder: 'Zariadenie',
          dataSource: store.zariadenia,
          displayExpr: 'NazovZariadenia',
          valueExpr: 'Id'
        }"
      >
        <dx-label :visible="true" text="Zariadenie" />
      </dx-item>
      <!-- <dx-item
        data-field="rememberMe"
        editor-type="dxCheckBox"
        :editor-options="{ text: 'Remember me', elementAttr: { class: 'form-text' } }"
      >
        <dx-label :visible="false" />
      </dx-item> -->
      <dx-button-item>
        <dx-button-options
          width="100%"
          type="default"
          template="signInTemplate"
          :use-submit-behavior="true"
        >
        </dx-button-options>
      </dx-button-item>
      <!-- <dx-item>
        <template #default>
          <div class="link">
            <router-link to="/reset-password">Forgot password?</router-link>
          </div>
        </template>
      </dx-item>
      <dx-button-item>
        <dx-button-options text="Create an account" width="100%" :on-click="onCreateAccountClick" />
      </dx-button-item> -->
      <template #signInTemplate>
        <div>
          <span class="dx-button-text">
            <dx-load-indicator v-if="loading" width="24px" height="24px" :visible="true" />
            <span v-if="!loading">Prihlásiť</span>
          </span>
        </div>
      </template>
    </dx-form>
  </form>
</template>

<script>
import DxLoadIndicator from 'devextreme-vue/load-indicator'
import DxForm, {
  DxItem,
  DxRequiredRule,
  DxLabel,
  DxButtonItem,
  DxButtonOptions
} from 'devextreme-vue/form'

import auth from '../auth'
import store from '@/store'

import { reactive, ref, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()

    const formData = reactive({
      email: '',
      password: '',
      zariadenieId: store.zariadenie?.Id
    })
    const loading = ref(false)

    watchEffect(() => {
      formData.zariadenieId = store.zariadenie?.Id
    })

    function onCreateAccountClick() {
      router.push('/create-account')
    }

    async function onSubmit() {
      const { email, password, zariadenieId } = formData
      loading.value = true
      const result = await auth.logIn(email, password, zariadenieId)
      if (!result.isOk) {
        loading.value = false
      } else {
        store.zariadenie = store.zariadenia.find((z) => z.Id === zariadenieId)
        router.push(route.query.redirect || '/')
      }
    }

    return {
      store,
      formData,
      loading,
      onCreateAccountClick,
      onSubmit
    }
  },
  components: {
    DxLoadIndicator,
    DxForm,
    DxRequiredRule,
    DxItem,
    DxLabel,
    DxButtonItem,
    DxButtonOptions
  }
}
</script>

<style lang="scss">
@import '../themes/generated/variables.base.scss';

.login-form {
  .link {
    text-align: center;
    font-size: 16px;
    font-style: normal;

    a {
      text-decoration: none;
    }
  }

  .form-text {
    margin: 10px 0;
    color: rgba($base-text-color, alpha($base-text-color) * 0.7);
  }
}
</style>
