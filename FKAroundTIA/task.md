# Завдання: Аналіз Faceplate Instance на екрані WinCC Unified через TIA Openness V21

## Вихідні дані

Існує готовий проєкт:

* C#
* WinForms
* .NET Framework 4.8
* Siemens TIA Openness V21

Проєкт вже вміє:

* запускати TIA Portal;
Необхідно розширити функціонал для роботи з Faceplate, розміщеними на конкретному екрані WinCC Unified.

---

# Мета

Створити інструмент, який:

1. Підключається до вже відкритого TIA Portal.
2. Знаходить WinCC Unified PC Runtime.
3. Знаходить екран з назвою "tpl".
4. Зчитує всі Faceplate Instance, розміщені на цьому екрані.
5. Відображає список знайдених Faceplate.
6. При виборі Faceplate показує його Interface Properties.
7. Архітектура повинна дозволяти в майбутньому змінювати властивості та прив'язки тегів.

---

# Важливе уточнення

Працювати потрібно НЕ з Faceplate Types проекту.

Працювати потрібно з конкретними Faceplate Instance, які фізично розміщені на екрані tpl.

Наприклад:

Motor01
Motor02
Valve01
Valve02

Кожен з них повинен розглядатися як окремий об'єкт.

---

# Етап 1. Пошук Runtime

Після підключення до проекту:

* знайти всі пристрої проекту;
* знайти всі WinCC Unified PC Runtime;
* якщо знайдено декілька Runtime, відобразити список для вибору;
* якщо знайдено один Runtime — використовувати його автоматично.

Створити DTO:

RuntimeInfo

Поля:

* Name
* DeviceName

---

# Етап 2. Пошук екрана tpl

У вибраному Runtime:

* рекурсивно обійти всі Screen Folder;
* знайти екран з назвою "tpl";
* пошук виконувати незалежно від вкладеності папок;
* якщо екран не знайдено — повідомити користувача.

Створити сервіс:

ScreenService

Метод:

FindScreenRecursive(runtime, "tpl")

---

# Етап 3. Зчитування всіх Screen Items

Після знаходження екрана:

виконати рекурсивний обхід усіх об'єктів екрана.

Необхідно підтримати:

* Screen Items
* Groups
* вкладені Groups
* Screen Windows
* вкладені Screen Windows

Обхід повинен бути повністю рекурсивним.

---

# Етап 4. Пошук Faceplate Instance

Під час обходу екрана необхідно знайти всі об'єкти, які є Faceplate Instance.

Для кожного знайденого Faceplate отримати:

* Instance Name
* Faceplate Type Name
* Parent Group
* Full Path
* X
* Y
* Width
* Height

Створити DTO:

FaceplateInstanceInfo

Поля:

* Name
* FaceplateTypeName
* ParentGroup
* FullPath
* X
* Y
* Width
* Height

---

# Формування FullPath

Приклад:

tpl/Motors/Motor01

або

tpl/Area1/Pumps/Pump03

Цей шлях потрібний для подальшого пошуку та модифікації об'єктів.

---

# Етап 5. Відображення Faceplate у UI

Ліва частина форми:

TreeView

Структура:

tpl
├─ Motor01
├─ Motor02
├─ Valve01
└─ Pump01

Текст вузла:

InstanceName [FaceplateType]

Приклад:

Motor01 [Motor]
Pump03 [Pump]

---

# Етап 6. Читання Interface Faceplate

При виборі Faceplate у TreeView необхідно прочитати його інтерфейс.

Зчитати максимально доступну інформацію про кожен Interface Item.

Створити DTO:

FaceplateInterfaceItemInfo

Поля:

* Name
* DisplayName
* Direction
* DataType
* CurrentValue
* Binding
* Comment

Якщо якесь поле недоступне через Openness API — залишати порожнім.

---

# Етап 7. Відображення інтерфейсу

Права частина форми:

DataGridView

Колонки:

* Name
* DisplayName
* Direction
* DataType
* CurrentValue
* Binding
* Comment

Після вибору Faceplate таблиця автоматично оновлюється.

---

# Етап 8. Підготовка до майбутнього редагування

Архітектуру необхідно будувати так, щоб у майбутньому можна було додати:

* зміну Interface Property;
* зміну Value;
* прив'язку HMI Tag;
* прив'язку PLC Tag;
* масове оновлення Faceplate.

Тому всі об'єкти Openness повинні бути інкапсульовані у сервісах.

UI не повинен напряму працювати з Siemens API.

---

# Архітектура проекту

Services/

TiaPortalService.cs

RuntimeService.cs

ScreenService.cs

FaceplateService.cs

Models/

RuntimeInfo.cs

FaceplateInstanceInfo.cs

FaceplateInterfaceItemInfo.cs

Forms/

MainForm.cs

---

# Вимоги до коду

Не використовувати:

* MVVM
* Dependency Injection Framework
* сторонні UI-фреймворки

Використовувати:

* WinForms
* DTO-моделі
* розділення UI та бізнес-логіки

---

# Обробка помилок

Передбачити обробку:

* TIA Portal не запущений;
* відсутній відкритий проект;
* Runtime не знайдено;
* екран tpl не знайдено;
* Faceplate не знайдено;
* помилки Openness API;
* помилки доступу до властивостей об'єктів.

Усі винятки логувати.

Користувачу показувати зрозуміле повідомлення через MessageBox.

---

# Очікуваний сценарій роботи

1. Користувач натискає кнопку "Load".
2. Програма підключається до відкритого TIA Portal.
3. Знаходить Runtime.
4. Знаходить екран tpl.
5. Рекурсивно сканує екран.
6. Формує список Faceplate Instance.
7. Відображає список у TreeView.
8. При виборі Faceplate показує Interface Properties у DataGridView.
