--Краткое описание--

Scenes/GameScene - основная сцены проекта
Scenes/Figures - сцена для настройки фигур
Data - данные для уровней

Процесс создания новых уровней.

Уровень представляет собой ScriptableObject типа LevelDataAsset. Для его создания есть
пункт меню Assets/Create/Figure Level. В списке Steps задаются данные для раундов - 
сама фигура и время на ее нарисовку.

Фигура - это ScriptableObject типа Polygon2dAsset. Для создания пункт меню Assets/Create/Figure.
При этом создается asset нужного типа и объект на сцене с компонентом Polygon2dAsset для
его визуально настройки. Для редактирования полигона (фигуры) у компонента есть
кнопки Add Point, Remove Point (удаляет выделенную точку). Перемещать точки можно в редакторе
с помощью Handles либо заданием точной позиции при выделении. После радактрования
нужно обязательно нажать Save Asset - для сохранения изменений на диск. Также в свойствах 
Polygon2dAsset задается SegmentOffest - опеределяет допустимое значение погрешности
при определении подобия фигуры (при сравнении идет проверка, чтобы точки лежали в данных
областях).

Далее созданные ассеты можно указывать в свойствах уровня (в списке Steps).

Для настройки допустимой погрешности у компонента GameManager есть поле PercentToComplete.


