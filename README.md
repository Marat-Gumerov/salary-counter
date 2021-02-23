## A test application with salary counting algorithm
[![Unit tests for BackEnd](https://github.com/Marat-Gumerov/salary-counter/actions/workflows/back-unit-tests.yml/badge.svg)](https://github.com/Marat-Gumerov/salary-counter/actions/workflows/back-unit-tests.yml) ![BackEnd Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/Marat-Gumerov/3375eaca628d8c57c854ebb56d2b6b14/raw/code-coverage.json)
### Implementation
#### Business logic
My implementation contains SalaryService - a class, which implements salary counting algorithm. This algorithm may be improved. For now, it creates a tree and traverses it using stacks. Other implementations could be based on recursion, or collecting salaries from parts. Collecting salary from parts means that when we count a salary for a worker, we don't search his subordinates, but we count all bonuses for all his chiefs. While traversing the list of workers, we will meet all subordinates and collect actual salary for every worker. Maybe this algorithm will be faster and will use less memory, but it can have bigger error, because we add small bonuses to big salaries. Also this algorithm has less readable code. Anyway, this task needs investigation to get best solution.

Data validators in SalaryService imply that all data, stored in application is valid. But for some cases, when data is valid, it is impossible to determine a salary. For example, when employee is hired before his chief. In this case we can choose a date, when employee already works in company, but his chief doesn't. If we want to determine a salary for their common chief - we can't determine a real chief for our employee.

The main set of data checks added to WorkerService.Save, it checks for cycles in subordination, existance of subordinates for employees and some other checks. So we can be sure, that data, stored in Dao layer is walid.

I think, the main thing I want to change in application, to make it more usefull, is not its algorithms. I think, when we meet such tasks, we should try to find a way to change business logic. Probably, the author of this salary determination algorithm did not see its real complexity. Even if we can implement this algorithm now, later it will be reused in lots of features of application, and we will get lots of bugs, because other features can be as complex as this one. So if we want a fast, lightweight and highly supportable application, we should try to reduce complexity of its business logic.

#### Backend
Architecture of backend divided to 3 projects: Service, Dao and Api. The main idea of such splitting is concentration of business logic in one place (Service) and isolate all external dependencies from it. Dao and Api layers used for integration with client and data storage. To check that all 3 layers integrated well, we need some Api tests. Unfortunately, for now we have only Unit tests for business logic.

Dao level uses Dictionaries for data storage implementation. It stores all data in RAM and looses it after restart. But Models are ready to be used in ORM frameworks, such as NHibernate, so we can replace current implementation with minimal refactoring. All models use Guid identifiers instead of integers. Using guid allowed me to not control automatic identifier incrementing.

#### Frontend
Application has a frontend, implemented using Angular. Fronend has less quality of code, because of my experiense in this technology. Frontend divided to 3 pages.

Home page created to count salary for all company. It contains a datepicker, so user can choose a date, for which salary will be counted. Second page is just a view, where user can see all positions for workers and their salary parameters. Third page shows a list of workers on specified date, also it has functions for adding new workers. Also, every worker on page has his own "Get salary" button, which could be used to count his salary on date specified for page.

#### Autotests
As I wrote before, the application does not contain Api and UI tests. I have experience in creating such tests, but I did not have enough time to develop them. Anyway, I think that this application must have Api tests, because Api and Dao layers have lots of code, which should be tested. On commercial projects I usually have only some critical Api tests for every feature, and it is usually enough.
