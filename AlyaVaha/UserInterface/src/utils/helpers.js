import DxNotify from 'devextreme/ui/notify'

const floatFormat = '#,##0.00'
const dateTimeFormat = 'd.M.yyyy H:mm'
const dateFormat = 'd.M.yyyy'
const timeFormat = 'H:mm'

const actualDate = () => new Date(new Date().setUTCHours(0, 0, 0, 0))

const filterOperations = ['=', 'between', '>=', '<=']

const filterStringOperations = ['startswith', 'contains', '=']

const toFloatNumber = (value, digits) =>
  value != null && value != undefined && !isNaN(value)
    ? parseFloat(value)
        .toFixed(digits)
        .toLocaleString('sk-SK', { minimumFractionDigits: 1, maximumFractionDigits: digits })
    : ''

const toDate = (value) => {
  if (!value) return ''
  const date = new Date(value).toLocaleDateString('sk-SK').replaceAll('. ', '.')
  if (date != 'Invalid Date') return date
  else return ''
}

const toTimezoneDate = (value) => {
  var date = new Date(value)
  var userTimezoneOffset = date.getTimezoneOffset() * 60000
  console.log(userTimezoneOffset)
  return new Date(date.getTime() - userTimezoneOffset)
}

const toCustomDate = (value) => {
  if (!value) return ''
  const date = new Date(value).toISOString().replace('T', ' ').replace('Z', '')
  if (date != 'Invalid Date') return date
  else return ''
}

const getTomorrow = (date) => {
  const tomorrow = new Date(date)
  tomorrow.setDate(tomorrow.getDate() + 1)
  return tomorrow
}

const getIncrementByMinute = (date) => {
  const increment = new Date(date)
  increment.setMinutes(increment.getMinutes() + 1)
  return increment
}

const notify = (message, type) => {
  DxNotify({
    message: message,
    type: type,
    displayTime: 5000,
    position: { at: 'top center', my: 'top center', offset: '0 3' },
    closeOnClick: true
  })
}

const shortNotify = (message, type) => {
  DxNotify({
    message: message,
    type: type,
    displayTime: 3000,
    position: { at: 'top center', my: 'top center', offset: '0 3' },
    closeOnClick: true
  })
}

export {
  actualDate,
  filterOperations,
  filterStringOperations,
  floatFormat,
  dateTimeFormat,
  dateFormat,
  timeFormat,
  toFloatNumber,
  toDate,
  toTimezoneDate,
  toCustomDate,
  getTomorrow,
  getIncrementByMinute,
  notify,
  shortNotify
}
