<script setup lang="ts">
import store from '@/store'
</script>

<template>
  <section>
    <h6 class="mb-1">Vstupy</h6>
    <div class="card card-body p-0">
      <div class="row m-0">
        <div class="card col-6" v-for="(value, index) in store.actualInputs" :key="index">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-7">{{ index.replace(/([A-Z0-9]+)/g, ' $1').trim() }}:</p>
            <p
              :class="'col fw-bold status contact-status status-' + value?.toString().toLowerCase()"
            >
              {{ value ? 'Zapnutý' : 'Vypnutý' }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

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
    @include status(#de8e8c);
  }
}
</style>
<style scoped lang="scss">
@use 'sass:math';

.contact-status {
  font-size: 13px;

  &::before {
    --diameter: 10px;

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
</style>
