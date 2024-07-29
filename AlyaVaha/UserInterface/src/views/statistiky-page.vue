<script setup>
import 'devextreme/data/odata/store'
import { reactive, watch, ref } from 'vue'
import { DxDateBox, DxSelectBox, DxButton } from 'devextreme-vue'
import { DxLoadIndicator } from 'devextreme-vue/load-indicator'
import Vue3Html2pdf from 'vue3-html2pdf'
import jsPDF from 'jspdf'

import store from '@/store'
import { sendCommand } from '@/commandHandler'
import { toFloatNumber, toDate, getTomorrow } from '@/utils/helpers'
import ReportPdf from './report-pdf.vue'

const html2pdfRef = ref()

const getCelkovyPocetNavazeni = () =>
  store.zariadenia?.reduce((acc, zariadenie) => acc + zariadenie.PocetNavazeni, 0)
const getCelkovyNavazeneMnozstvo = () =>
  store.zariadenia?.reduce((acc, zariadenie) => acc + zariadenie.NavazeneMnozstvo, 0)
const getCelkovyNavazenyPocetDavok = () =>
  store.zariadenia?.reduce((acc, zariadenie) => acc + zariadenie.NavazenyPocetDavok, 0)

const getZariadenia = () => [{ Id: 0, NazovZariadenia: '(Všetky)' }, ...store.zariadenia]
const getMaterialy = () => [{ Id: 0, NazovMaterialu: '(Všetky)' }, ...store.aktivneMaterialy]
const getZasobnikyDoVahy = () => [{ Id: 0, NazovZasobnika: '(Všetky)' }, ...store.zasobnikyDoVahy]
const getZasobnikyZVahy = () => [{ Id: 0, NazovZasobnika: '(Všetky)' }, ...store.zasobnikyZVahy]

sendCommand('GetZariadenia')

const state = reactive({
  pocetNavazeni: getCelkovyPocetNavazeni(),
  navazeneMnozstvo: getCelkovyNavazeneMnozstvo(),
  navazenyPocetDavok: getCelkovyNavazenyPocetDavok(),
  zariadenia: getZariadenia(),
  materialy: getMaterialy(),
  zasobnikyDoVahy: getZasobnikyDoVahy(),
  zasobnikyZVahy: getZasobnikyZVahy(),
  zariadenie: 0,
  filterOptions: {
    datumOd: null,
    datumDo: null,
    material: 0,
    odkial: 0,
    kam: 0
  },
  filterOptionsStr: {
    zariadenie: '',
    datumOd: '',
    datumDo: '',
    material: '',
    odkial: '',
    kam: ''
  }
})

watch(
  () => store.zariadenia,
  () => {
    state.zariadenia = getZariadenia()
    getPrecalculatedStatistics()
  }
)

watch(
  () => store.aktivneMaterialy,
  () => {
    state.materialy = getMaterialy()
  }
)

watch(
  () => store.zasobnikyDoVahy,
  () => {
    state.zasobnikyDoVahy = getZasobnikyDoVahy()
  }
)

watch(
  () => store.zasobnikyZVahy,
  () => {
    state.zasobnikyZVahy = getZasobnikyZVahy()
  }
)

watch(
  () => store.statistiky,
  () => {
    state.pocetNavazeni = store.statistiky?.[0]
    state.navazeneMnozstvo = store.statistiky?.[1]
    state.navazenyPocetDavok = store.statistiky?.[2]
  }
)

// function getCobminedFilter, which builds a filter object based on the state.filterOptions
function getCombinedFilter() {
  const filter = []
  if (state.zariadenie) {
    filter.push(['ZariadenieId', '>=', state.zariadenie])
  }
  if (state.filterOptions.datumOd) {
    var date = new Date(state.filterOptions.datumOd)
    filter.push(['DatumStartu', '>=', date])
  }
  if (state.filterOptions.datumDo) {
    var dateAfter = getTomorrow(state.filterOptions.datumDo)
    filter.push(['DatumStartu', '<', dateAfter])
  }
  if (state.filterOptions.material) {
    filter.push(['MaterialId', '=', state.filterOptions.material])
  }
  if (state.filterOptions.odkial) {
    filter.push(['OdkialId', '=', state.filterOptions.odkial])
  }
  if (state.filterOptions.kam) {
    filter.push(['KamId', '=', state.filterOptions.kam])
  }

  return filter
}

function getPrecalculatedStatistics() {
  state.pocetNavazeni = getCelkovyPocetNavazeni()
  state.navazeneMnozstvo = getCelkovyNavazeneMnozstvo()
  state.navazenyPocetDavok = getCelkovyNavazenyPocetDavok()
}

function getStatistics() {
  // we can find statistics instantly if no filter is applied or just zariadenie is selected
  if (Object.values(state.filterOptions).every((value) => !value)) {
    if (!state.zariadenie) {
      getPrecalculatedStatistics()
    } else {
      const selectedZariadenie = store.zariadenia.find(
        (zariadenie) => zariadenie.Id === state.zariadenie
      )
      if (selectedZariadenie) {
        state.pocetNavazeni = selectedZariadenie.PocetNavazeni
        state.navazeneMnozstvo = selectedZariadenie.NavazeneMnozstvo
        state.navazenyPocetDavok = selectedZariadenie.NavazenyPocetDavok
      }
    }
  } else {
    store.statistikyLoading = true
    const dataSourceLoadOptions = {}
    dataSourceLoadOptions.Filter = getCombinedFilter()
    dataSourceLoadOptions.TotalSummary = [
      { Selector: 'NavazeneMnozstvo', SummaryType: 'sum' },
      { Selector: 'NavazenyPocetDavok', SummaryType: 'sum' }
    ]
    dataSourceLoadOptions.RequireTotalCount = true
    dataSourceLoadOptions.Skip = 0
    dataSourceLoadOptions.Take = 1
    sendCommand('GetStatistiky', dataSourceLoadOptions)
  }
}

function filterToStr() {
  const filterStr = {
    zariadenie: state.zariadenie
      ? state.zariadenia.find((zariadenie) => zariadenie.Id === state.zariadenie).NazovZariadenia
      : '(Všetky)',
    datumOd: toDate(state.filterOptions.datumOd),
    datumDo: toDate(state.filterOptions.datumDo),
    material: state.filterOptions.material
      ? store.aktivneMaterialy.find((material) => material.Id === state.filterOptions.material)
          .NazovMaterialu
      : '(Všetky)',
    odkial: state.filterOptions.odkial
      ? store.zasobnikyDoVahy.find((zasobnik) => zasobnik.Id === state.filterOptions.odkial)
          .NazovZasobnika
      : '(Všetky)',
    kam: state.filterOptions.kam
      ? store.zasobnikyZVahy.find((zasobnik) => zasobnik.Id === state.filterOptions.kam)
          .NazovZasobnika
      : '(Všetky)'
  }
  state.filterOptionsStr = filterStr
}

function exportToPdf() {
  try {
    filterToStr()
    html2pdfRef.value.generatePdf()
  } catch (e) {
    console.log(e)
  }
}
</script>

<template>
  <div class="d-flex justify-content-between" style="margin-bottom: -16px">
    <div class="d-flex justify-content-start">
      <h2 class="content-block">Štatistiky</h2>
    </div>
    <div>
      <button type="button" class="btn btn-primary h-50 m-3 mr-0" @click="exportToPdf">
        Exportovať do PDF
      </button>
    </div>
  </div>
  <div class="my-3 w-100">
    <div class="card card-body">
      <div class="row">
        <div class="col">
          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Zariadenie:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.zariadenia"
                v-model:value="state.zariadenie"
                value-expr="Id"
                display-expr="NazovZariadenia"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Dátum od:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxDateBox
                v-model="state.filterOptions.datumOd"
                date-serialization-format="yyyy-MM-dd"
                :input-attr="{ 'aria-label': 'Date' }"
                type="date"
                :show-clear-button="true"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Dátum do:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxDateBox
                v-model="state.filterOptions.datumDo"
                date-serialization-format="yyyy-MM-dd"
                :input-attr="{ 'aria-label': 'Date' }"
                type="date"
                :show-clear-button="true"
              />
            </div>
          </div>
        </div>

        <div class="col">
          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Materiál:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.materialy"
                v-model:value="state.filterOptions.material"
                value-expr="Id"
                display-expr="NazovMaterialu"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Odkiaľ:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.zasobnikyDoVahy"
                v-model:value="state.filterOptions.odkial"
                value-expr="Id"
                display-expr="NazovZasobnika"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Kam:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.zasobnikyZVahy"
                v-model:value="state.filterOptions.kam"
                value-expr="Id"
                display-expr="NazovZasobnika"
              />
            </div>
          </div>
        </div>

        <div class="row ml-3 my-2">
          <div class="col-2 mt-2">
            <DxButton text="Načítať" type="success" @click="getStatistics" />
          </div>
        </div>

        <div
          v-if="store.statistikyLoading"
          class="mt-5 mb-3 d-flex justify-content-center align-items-center"
        >
          <DxLoadIndicator :height="96" :width="96" />
        </div>

        <div v-if="!store.statistikyLoading" class="row mt-5 mb-3 mx-2">
          <div class="col">
            <div class="card">
              <div class="row h-100 d-flex align-items-center py-2 pl-3">
                <h5 class="card-title">Počet navažovaní</h5>
                <p class="card-text fw-bold fs-3">
                  {{ state.pocetNavazeni }}
                </p>
              </div>
            </div>
          </div>
          <div class="col">
            <div class="card">
              <div class="row h-100 d-flex align-items-center py-2 pl-3">
                <h5 class="card-title">Navážené množstvo (kg)</h5>
                <p class="card-text fw-bold fs-3">
                  {{ toFloatNumber(state.navazeneMnozstvo, 1) }}
                </p>
              </div>
            </div>
          </div>
          <div class="col">
            <div class="card">
              <div class="row h-100 d-flex align-items-center py-2 pl-3">
                <h5 class="card-title">Navážený počet dávok</h5>
                <p class="card-text fw-bold fs-3">
                  {{ state.navazenyPocetDavok }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div>
    <vue3-html2pdf
      :show-layout="false"
      :float-layout="true"
      :enable-download="true"
      :preview-modal="false"
      :paginate-elements-by-height="1400"
      filename="AlyaVaha-Statistiky"
      :pdf-quality="2"
      :manual-pagination="false"
      pdf-format="a4"
      :pdf-margin="10"
      pdf-orientation="landscape"
      pdf-content-width="1100px"
      ref="html2pdfRef"
    >
      <template v-slot:pdf-content>
        <ReportPdf
          :filterStr="state.filterOptionsStr"
          :pocetNavazeni="state.pocetNavazeni"
          :navazeneMnozstvo="state.navazeneMnozstvo"
          :navazenyPocetDavok="state.navazenyPocetDavok"
        />
      </template>
    </vue3-html2pdf>
  </div>
</template>
