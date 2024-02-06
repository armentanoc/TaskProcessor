# TaskProcessor 🚀

## Visão Geral

Bem-vindo ao TaskProcessor, o seu processador de tarefas paralelo! 🤖 
Este programa gerencia tarefas e subtarefas utilizando a poderosa estrutura `Task` seguindo a arquitetura DDD (Design Orientado a Domínio) e utilizando metodologias como `Generics`, `Injeção de Dependência`, `Async/Await`, `Task`, `Entity Framework`, `IConfiguration`, `IServiceProvider`, etc.
Aqui está uma breve visão geral:

## Projetos 🚀

1. **TaskProcessor.Domain** 🌌
   - Classes de modelo (`BaseEntity`, `SubTaskEntity`, `TaskEntity`, `TaskPriorityEnum` e `TaskStatusEnum`).
   - Interfaces que orientam a Infraestrutura (`IRepository`, `IRepositoryTaskEntity`, `IRepositorySubTaskEntity`).

3. **TaskProcessor.Application** 🚀
   - Contém os serviços da aplicação (`SubTaskService`, `TaskService`, `TaskExecutionService`).
   - Além de uma classe auxiliar (`ServiceHelper`) para apoiar esses serviços.

4. **TaskProcessor.Infra** 🌠
   - Armazenamento em memória, mas pronto para explorar outros bancos de dados com o `EntityFrameworkCore` 🌐.
   - Centro de controle (`AppDbContext`), usando a potência do `EntityFrameworkCore`.
   - Repositórios genéricos (`EFRepository`) e específicos (`EFRepositorySubTaskEntity`, `EFRepositoryTaskEntity`).

5. **TaskProcessor.Presentation** 👽
   - Configurações ajustadas lendo o `appsettings.json` através do padrão `IConfiguration`.
   - Injeção de dependência - contexto de database, serviços e demais itens importantes utilizando `IServiceProvider`.
   - Interface do usuário no console para interações com o usuário.

## Jornada das `TaskEntity` 🌌

- Prioridades aleatórias (`High`, `Medium`, `Low`) decidem a ordem de execução.
- Controle de estados para `TaskEntity` e `SubTaskEntity` (`Created`, `Scheduled`, `InProgress`, `Paused`, `Completed`, `Cancelled`) 
- Pause uma tarefa antes do lançamento, transformando seu status em `Paused`.
- Durante a execução, retome a execução com o agendamento, modificando o status para `Scheduled`.
- Atrasos simulam a execução de cada tarefa (`Task.Delay`).

## Logging de informações e Principais Recursos 🛰️

- Criação de um log do serviço de execução (`log_execution_service.txt`) no qual é possível acompanhar o que está ocorrendo na aplicação, como por exemplo:
- A execução verdadeiramente paralela e assíncrona de `TaskEntity` e `SubTaskEntity`
- Início da execução de uma `TaskEntity`
```
[STARTED] Executing Task: [INFO] Id 4 - Priority High
Subtasks: SubTask Id: 121, Duration: 198s, SubTask Id: 122, Duration: 174s, SubTask Id: 123, Duration: 195s, SubTask Id: 124, Duration: 225s, SubTask Id: 125, Duration: 141s, SubTask Id: 126, Duration: 298s, SubTask Id: 127, Duration: 281s, SubTask Id: 128, Duration: 158s, SubTask Id: 129, Duration: 213s, SubTask Id: 130, Duration: 299s, SubTask Id: 131, Duration: 238s, SubTask Id: 132, Duration: 278s, SubTask Id: 133, Duration: 127s, SubTask Id: 134, Duration: 124s, SubTask Id: 135, Duration: 130s At 2024-02-06 23:47:49.368
```

- Início da execução de uma SubTaskEntity
```
[STARTED] Executing SubTask: [INFO] 138 At 2024-02-06 23:47:49.398 Duration: 00:02:52 ElapsedTime: 00:00:00
```

- Atualizações de `ElapsedTime` para `SubTaskEntity`
```
[UPDATE] SubTask Updated: [INFO] 64 At 2024-02-06 22:09:55.863 Duration: 00:03:33 ElapsedTime: 00:00:13
```

- Progresso da `TaskEntity`
```
[TASK PROGRESS] Progress: 1/29 subtasks completed for Task 5
```

- Finalização da `SubTaskEntity`:
```
[COMPLETED] SubTask Completed: [INFO] 164 At 2024-02-06 23:47:50.524 Duration: 00:02:06 ElapsedTime: 00:02:06
```

- Finalização da `TaskEntity`
```
[COMPLETED] Task Completed: [INFO] Id 5 - Priority Medium
Subtasks: SubTask Id: 136, Duration: 219s, SubTask Id: 137, Duration: 181s, SubTask Id: 138, Duration: 172s, SubTask Id: 139, Duration: 280s, SubTask Id: 140, Duration: 241s, SubTask Id: 141, Duration: 103s, SubTask Id: 142, Duration: 243s, SubTask Id: 143, Duration: 201s, SubTask Id: 144, Duration: 237s, SubTask Id: 145, Duration: 234s, SubTask Id: 146, Duration: 299s, SubTask Id: 147, Duration: 119s, SubTask Id: 148, Duration: 220s, SubTask Id: 149, Duration: 149s, SubTask Id: 150, Duration: 198s, SubTask Id: 151, Duration: 282s, SubTask Id: 152, Duration: 293s, SubTask Id: 153, Duration: 195s, SubTask Id: 154, Duration: 145s, SubTask Id: 155, Duration: 207s, SubTask Id: 156, Duration: 277s, SubTask Id: 157, Duration: 260s, SubTask Id: 158, Duration: 266s, SubTask Id: 159, Duration: 141s, SubTask Id: 160, Duration: 290s, SubTask Id: 161, Duration: 297s, SubTask Id: 162, Duration: 107s, SubTask Id: 163, Duration: 172s, SubTask Id: 164, Duration: 126s At 2024-02-06 23:47:55.412
```


## Como Executar

Para utilizar este projeto, siga os passos abaixo:

1. Clone o repositório para o seu ambiente local usando o seguinte comando no terminal:

   ```bash
   git clone https://github.com/armentanoc/TaskProcessor.git
   
Certifique-se de ter o ambiente de desenvolvimento C# configurado. Após clonar o repositório, compile e execute o código-fonte no seu ambiente de desenvolvimento preferido.

  ```bash
  dotnet run
  ```

Divirta-se explorando, modificando e experimentando! 🌌🌠
