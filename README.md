# LibraryAPI
REST API aplikacji bibliotecznej, utworzonej w ASP.net core 5.

### Link do API
https://library-api-app.azurewebsites.net/swagger

### Logowanie
Po wpisaniu emaila i hasła metoda logowania zwraca id użytkownika, roleId oraz wytworzony dla niego token JWT, token wygasa po 15 dniach.

### Autoryzacja JWT Token
Aby zdobyć uprawnienia danej roli należy  w headers, w kluczu wprowadzic "Authorization" oraz w wartości Token z przedrostkiem Bearer.


### Paginacja 
W aplikacji zastosowana jest paginacja, czyli stronnicowanie stron. Aby móc zwrócić  książki należy użyć parametru pageSize oraz pageNumber.
PageSize musi wynosić 5,10 lub 15.

Przykład

`/api/books?pageSize=15&pageNumber=1`

### Wyszukiwanie
Aby wyszukać książkę po danej frazie należy dodać parametr searchPhrase

Przykład

`/api/books?searchPhrase=python&pageSize=15&pageNumber=1`

### Sortowanie
W Api możliwe jest sortowanie książek poprzez kolumny:"BookName,BookDescription,Category,PublisherName,PublishDate", rosnąco lub malejąco (ASC, DESC lub we swaggerze 1 i 0).
Aby posortować wyniki według kolumn należy dodać parametr sortBy, jeżeli chcemy dodatkowo posortować malejąco lub rosnąco dodajemy parametr sortDirection.

Przykład

`/api/books?searchPhrase=python&pageSize=15&pageNumber=1&sortBy=Category&sortDirection=DESC`

## Metody przekazywania danych

| Wywołania GET  | Opis | 
| ------------- | ------------ | 
| ../api/books  | Książki - wszystkie  | 
| ../api/books/{id} | Książki - pojedyncza wartość  | | 
| ../api/books/user/{userid} | User (pracownik i admin dla wszystkich, zwykły użytkownik dla siebie) - pobranie listy wypożyczonych książek dla ID usera  | 
| ../api/users/{id}  | User - pobranie danych po ID Usera (zwykły użytkownik tylko siebie, admin i pracownik wszystkich) | 
| ../api/users | User (pracownik,admin) - pobranie wszystkich userów |

| Wywołania PUT  | Opis | 
| ------------- | ------------ | 
| ../api/users/{id}  | User  - zmiana Imienia, nazwiska, emaila (każdy dla siebie, admin może dla wszystkich) | 
| ../api/books/{id} | User (pracownik,admin) - edycja książki  | 



| Wywołania PATCH  | Opis | 
| ------------- | ------------ | 
|../api/users/changePassword/{id}  | zmiana hasła (każdy dla siebie, admin może dla wszystkich)  |   | 
|../api/books/reservation/{id} | User - rezerwacja  | 
|../api/books/borrow/{id} | Pracownik - wypożyczenie ksiązki dla użytkownika |
|../api/books/return/{id} | Pracownik - zwrot ksiązki od użytkownika  |

| Wywołania POST  | Opis | 
| ------------- | ------------ | 
| ../api/books  | User (pracownik,admin) - dodanie nowej książki  | 
| ../api/users | User (admin) - dodanie nowego użytkownika, pracownika lub admina | 
| ../api/account/register | rejestracja użytkownika |
| ../api​/account​/login    |  logowanie użytkownika |

| Wywołania DELETE  | Opis | 
| ------------- | ------------ | 
| ../api/books/{id}  | User (pracownik,admin) - usuwanie książki  | 
| ../api/users{id} | usuwanie użytkownika(Usunać może każdy siebie, admin może wszystkich) | 
