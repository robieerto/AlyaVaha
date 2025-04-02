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

const toTime = (value) => {
  if (!value) return ''
  const date = new Date(value).toLocaleTimeString('sk-SK').replace(':00', '')
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

const formatDateTime = (dateTime) => {
  if (!dateTime) return ''
  return dateTime.replace(/(\d{2}\.\d{2}\.\d{4})(\d{2}:\d{2}:\d{2})/, '$1 $2')
}

const formatTime = (secondsTime) => {
  if (!secondsTime) return ''
  const hours = Math.floor(secondsTime / 3600)
  const minutes = Math.floor((secondsTime % 3600) / 60)
  const seconds = secondsTime % 60
  return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`
}

const parseCustomDate = (dateString) => {
  // Split the date and time parts
  const formattedDateString = formatDateTime(dateString)
  const [datePart, timePart] = formattedDateString.split(' ')

  // Extract day, month, and year from the date part
  const [day, month, year] = datePart.split('.').map(Number)

  // Extract hours, minutes, and seconds from the time part
  const [hours, minutes, seconds] = timePart.split(':').map(Number)

  // Construct the Date object (months are 0-based in JavaScript)
  return new Date(year, month - 1, day, hours, minutes, seconds)
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
  toTime,
  toTimezoneDate,
  toCustomDate,
  getTomorrow,
  getIncrementByMinute,
  formatDateTime,
  formatTime,
  parseCustomDate,
  notify,
  shortNotify
}
