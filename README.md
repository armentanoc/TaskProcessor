# DDDWithSQLite

1. N�o � necess�rio login/autentica��o, j� abrimos o programa com o usu�rio logado

2. O programa � um processador de tarefas que processa paralelamente determinadas tarefas
- Crit�rios
	- Estabelecer crit�rios para quais tarefas executar primeiro
	- Tarefas de alta prioridade devem ser processadas primeiro
	- Deve ser poss�vel cancelar uma tarefa em execu��o
	- As tarefas devem ser armazenadas em um banco de dados (SQLite)
- Subtarefas
	- Cada tarefa pode ter subtarefas 
- Tempo
	- A sugest�o do professor � gerar tarefas com tempo aleat�rio - elas devem ter tempo diferentes (ver o thresold)
	- Devemos estabelecer um delay para simular o processamento de cada tarefa (Task.Delay) - ver tempo que o professor falou
	- O armazenamento deve ser persistente e, fechando o programa, ao reiniciar o sistema deve reiniciar de onde parou
- Forma de processamento
	- As tarefas devem ser processadas em paralelo (Parallel.ForEach)
	- As tarefas devem ser processadas de maneira ass�ncrona (async/await)
- Estados de tarefa
	- As tarefas devem ter estados (criadas, agendadas, em execu��o, conclu�das, etc.) - ver nomenclatura certinha que o prof. passou 
	- As tarefas devem ser armazenadas em um banco de dados (SQLite)

3. O que o programa deve ter? 
	- Usar Generics
	- Inje��o de depend�ncia
	- Async/Await
	- Parallel
	- Readme

4. A configura��o deve ser feita em um arquivo de configura��o
	- A quantidade de tarefas dever� ser uma configura��o
	- A quantidade que cada subtarefas pode ter deve ser uma configura��o
	- O tipo de armazenamento (ex.: Sqlite) e o nome do database dever� ser uma configura��o 
