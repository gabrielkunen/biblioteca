# biblioteca-api
Esta API é um projeto pessoal para simular uma biblioteca, com CRUD de usuários, autores, livros. Empréstimo, devolução, renovação de livros e um relatório do status do acervo.

Como se trata apenas de um ambiente de demonstração, o ambiente é reiniciado a cada 2 horas, junto com a limpeza do banco de dados.

Grande parte dos endpoints necessitam de autorização, então é necessário logar pelo método /funcionarios/logar com email e senha para obter o bearer token de acesso dos outros endpoints.

# Endpoints

Os endpoints **/autores**, **/generos**, **/livros** e **/usuarios** são basicamente um CRUD, mas o /livros possui um endpoint de relatório usando QuestPDF que mostra o status dos livros na biblioteca.

Há também um endpoint de **/livros** que faz upload do relatório direto no bucket da Cloudflare R2, apesar de funcional, está desativado no ambiente por segurança.

O endpoint **/funcionarios/logar** é utilizado para gerar o bearer token jwt de acesso aos outros endpoints.

Os livros podem ser emprestados, devolvidos e renovados nos endpoints **/emprestimos**, eles possuem várias regras de negócio, validando se o livro encontra-se disponível para empréstimo, se o usuário ainda possui limite, se já passou o prazo de devolução e etc.