* LEFT OFF => Fix bugs
	* Razor components Entity should be Dto type. This will probably fix a few bugs
	* Add new row should not cause modal to close when form is valid
	* On edit, if I add another row, then clear out all rows, then hit add new row or save, there's a few issues here
* LEFT OFF => Enhance error messaging
	* Input form should be red
	* I can maybe create a component similar to ValidationMessage
	* Test what happens when multiple errors happen, such as max length and valid email
	* Implement FluentValidations
	* Implement global error. Probably put near submit button
* Convert razor Entity property to use the Dto types. I think this is what I want to do. Might need to create a mapping profile or something too
* Remove unused endpoints such as /auth
* Setup logging. UpdateToDo.cs
* I feel like adding a table to track ToDo priorities might be nice
* When getting all the types, this is NOT really ideal. It gets everything lol. See if I can narrow this down. 2 places call this I think
* At the very end, I can maybe just delete the migrations folder and have 1 migration. Might be confusing though not sure
* Maybe come back and create a UpdateEntity class. Use in ToDoForm
* ToDo: DatabaseContext.ConfigureConventions seems like a good spot to put my custom model building logic
* ToDo: Probably a good idea to add SortOrder. Even though I really don't want to add drag and drop functionality, just adding SortOrder will be nice to ToDo and ToDoItem
* Add support for recurring ToDos
* MapMethods() has a method after this call called CacheOutput, oooo
* Remove GetToDoEditForm. Just keeping for now for reference
* I'll want to create EntityUpdater. Use in UpdateToDo.cs and the quick update endpoints

* ToDo listing
* New ToDo button
	* Htmx to get EditForm
* Edit button will call API to get EditForm html
	* User can edit ToDo.Title and ToDoItems.Desciptions
* Htmx will be used for quick update options
	* Quick update Status
	* Quick delete ToDo and ToDoItem