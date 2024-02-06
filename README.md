# TaskProcessor

- O programa é um processador de tarefas que processa paralelamente determinadas tarefas utilizando a estrutura `Task` para nortear a execução de `TaskEntity`e suas `SubTaskEntity`; 
- O programa foi desenvolvido com base na arquitetura DDD, com os seguintes projetos: 

1. TaskProcessor.Domain (sem dependências) -> é uma ClassLib com classes de modelo (BaseEntity, SubTaskEntity, TaskEntity, TaskPriorityEnum e TaskStatusEnum) e interfaces que guiam a implementação dos repositórios geral e específicos (IRepository, IRepositorySubTaskEntity, IRepositoryTaskEntity).
2. TaskProcessor.Application (depende do Domain e da Infra) -> é uma ClassLib com classes de serviço (SubTaskService, TaskService, TaskExecutionService e uma classe com métodos de apoio chamada ServiceHelper)
3. TaskProcessor.Infra (depende do Domain) -> é uma ClassLib que tem um AppDbContext que herda do DbContext do EntityFrameworkCore, gerando os DbSet<TaskEntity> e DbSet<SubTaskEntity> e os repositórios propriamente dito, genérico (EFRepository) e específicos (EFRepositorySubTaskEntity, EFRepositoryTaskEntity)
4. TaskProcessor.Presentation (depende de Application) -> realiza a configuração do aplicativo lendo do arquivo appsettings.json com o IConfiguration, bem como a injeção de dependência utilizando o IServiceProvider, aborda o build das configurações, a criação de escopo para executar a aplicação e a UI propriamente dita exibida em uma Aplicação Console.

- As TaskEntity são aleatoriamentes marcadas com prioridade `High`, `Medium` ou `Low`, sendo executadas nessa ordem;
- É possível pausar uma tarefa ainda não executada, modificando seu status para `Paused`;
- Enquanto as tarefas estão sendo executadas, é possível resumir o agendamento de uma tarefa, modificando seu status para `Reschedule`;
- Foi utilizado Armazenamento em Memória, mas a estrutura do programa está preparada para configuração do armazenamento com banco de dados através do Entity 
- Foi estabelecido um delay aleatório para simular o processamento de cada tarefa (Task.Delay);
- É especificado no arquivo de configuração (`appsettings.json`) a quantidade de tarefas a serem executadas ao mesmo tempo e a quantidade aleatória de tarefas a serem geradas;
- Essas tarefas são executadas paralelamente de maneira assíncrona (`async`/`await`, com a execução de cada uma das subtarefas e salvamento do progresso parcial; 
- As tarefas têm estados (`Created`, `Scheduled`, `InProgress`, `Paused`, `Completed`, `Cancelled`);
- O programa utiliza: Generics, Injeção de dependência, Async/Await, Execução paralela (Task.WhenAll)
