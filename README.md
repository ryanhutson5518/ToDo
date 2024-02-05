* LEFT OFF => Enhance error messaging
	* Input form should be red
	* I can maybe create a component similar to ValidationMessage
	* Test what happens when multiple errors happen, such as max length and valid email
	* Implement FluentValidations
	* Implement global error. Probably put near submit button
* Remove unused endpoints such as /auth
* I feel like adding a table to track ToDo priorities might be nice
* At the very end, I can maybe just delete the migrations folder and have 1 migration. Might be confusing though not sure
* Maybe come back and create a UpdateEntity class. Use in ToDoForm
* ToDo: DatabaseContext.ConfigureConventions seems like a good spot to put my custom model building logic
* ToDo: Probably a good idea to add SortOrder. Even though I really don't want to add drag and drop functionality, just adding SortOrder will be nice to ToDo and ToDoItem
* Add support for recurring ToDos
* MapMethods() has a method after this call called CacheOutput, oooo
* Remove GetToDoEditForm. Just keeping for now for reference

* ToDo listing
* New ToDo button
	* Htmx to get EditForm
* Edit button will call API to get EditForm html
	* User can edit ToDo.Title and ToDoItems.Desciptions
* Htmx will be used for quick update options
	* Quick update Status
	* Quick delete ToDo and ToDoItem