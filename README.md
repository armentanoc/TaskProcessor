# DDDWithSQLite

1. Não é necessário login/autenticação, já abrimos o programa com o usuário logado

2. O programa é um processador de tarefas que processa paralelamente determinadas tarefas
- Critérios
	- Estabelecer critérios para quais tarefas executar primeiro
	- Tarefas de alta prioridade devem ser processadas primeiro
	- Deve ser possível cancelar uma tarefa em execução
	- As tarefas devem ser armazenadas em um banco de dados (SQLite)
- Subtarefas
	- Cada tarefa pode ter subtarefas 
- Tempo
	- A sugestão do professor é gerar tarefas com tempo aleatório - elas devem ter tempo diferentes (ver o thresold)
	- Devemos estabelecer um delay para simular o processamento de cada tarefa (Task.Delay) - ver tempo que o professor falou
	- O armazenamento deve ser persistente e, fechando o programa, ao reiniciar o sistema deve reiniciar de onde parou
- Forma de processamento
	- As tarefas devem ser processadas em paralelo (Parallel.ForEach)
	- As tarefas devem ser processadas de maneira assíncrona (async/await)
- Estados de tarefa
	- As tarefas devem ter estados (criadas, agendadas, em execução, concluídas, etc.) - ver nomenclatura certinha que o prof. passou 
	- As tarefas devem ser armazenadas em um banco de dados (SQLite)

3. O que o programa deve ter? 
	- Usar Generics
	- Injeção de dependência
	- Async/Await
	- Parallel
	- Readme

4. A configuração deve ser feita em um arquivo de configuração
	- A quantidade de tarefas deverá ser uma configuração
	- A quantidade que cada subtarefas pode ter deve ser uma configuração
	- O tipo de armazenamento (ex.: Sqlite) e o nome do database deverá ser uma configuração 
