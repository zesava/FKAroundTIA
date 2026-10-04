# Модифікація Faceplate Analyzer

Поточна реалізація:

* знаходить екран tpl;
* знаходить усі Faceplate Instance;
* показує кожен екземпляр окремим вузлом TreeView;
* при виборі екземпляра відображає його Interface Properties.

Необхідно змінити логіку роботи.

---

# Нова логіка TreeView

Замість відображення кожного екземпляра Faceplate окремо необхідно виконати групування за Faceplate Type.

Приклад.

Було:

Pump_1
Pump_2
Pump_3
Valve_1
Valve_2

Стало:

Pump [3]
Valve [2]

де число у квадратних дужках — кількість екземплярів даного типу на екрані.

---

# Нова модель

Створити DTO:

FaceplateTypeGroupInfo

Поля:

* TypeName
* Count
* Instances

де Instances містить список усіх FaceplateInstanceInfo даного типу.

---

# Завантаження даних

Після сканування екрана:

1. Зібрати всі Faceplate Instance.
2. Згрупувати по Faceplate Type Name.
3. Заповнити колекцію FaceplateTypeGroupInfo.
4. Відобразити групи у TreeView.

---

# Відображення TreeView

Текст вузла:

<TypeName> [<Count>]

Приклад:

Pump [3]
Valve [10]
Motor [25]

У Tag вузла зберігати FaceplateTypeGroupInfo.

---

# Вибір групи Faceplate

При виборі вузла TreeView:

отримати всі екземпляри Faceplate даного типу.

Приклад:

Pump [3]

містить:

Pump_1
Pump_2
Pump_3

---

# Відображення інтерфейсу

Справа у DataGridView показувати список Interface Properties.

Інтерфейс зчитувати з першого екземпляра групи.

Припускається, що всі екземпляри одного Faceplate Type мають однаковий набір Interface Properties.

---

# Нова таблиця

Колонки:

* PropertyName
* DataType
* Value
* Binding
* Modified

де:

PropertyName — назва Interface Property.

DataType — тип даних.

Value — поточне значення.

Binding — поточна прив'язка тега.

Modified — прапорець зміни.

---

# Редагування

Колонки Value та Binding повинні бути доступні для редагування.

Після редагування значення:

Modified = true.

---

# Масове застосування

Додати кнопку:

Apply To All

При натисканні:

для кожного Faceplate Instance вибраної групи:

* знайти відповідну Interface Property;
* записати нове значення;
* або оновити Binding.

Приклад:

Було:

Pump_1.StateTag = Motor_1.State
Pump_2.StateTag = Motor_2.State
Pump_3.StateTag = Motor_3.State

Після редагування:

StateTag = PLC_Global.State

Програма повинна записати це значення у всі екземпляри групи.

---

# Сервісний шар

Створити окремий сервіс:

FaceplateGroupService

Методи:

GetFaceplateGroups()

GetInterfaceProperties(FaceplateTypeGroupInfo group)

ApplyChanges(FaceplateTypeGroupInfo group)

---

# Важлива вимога

Усі зміни виконувати через Openness API.

UI не повинен напряму працювати з об'єктами Siemens.

Усі об'єкти Siemens повинні залишатися всередині сервісів.


