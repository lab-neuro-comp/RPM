# Matrizes Progressivas de Raven

Este manual foi escrito pensado no aplicador do teste. Ele descreve como a pasta do aplicativo está organizada,  como criar um teste e como realizar um teste.


## Estrutura do programa

A pasta do Raven contém os seguintes itens:

- Assets: é a pasta que contém os testes em si. Todas as imagens e informações de um teste deverão constar aqui.
- Results: é pasta que contém os resultados dos testes já realizados.
- Raven.exe: é o programa.
- README.txt: este arquivo de ajuda que você está lendo.
- FSharp.Core.DLL e Infra.DLL: são arquivos necessários para que o programa rode.


### Pasta `assets` ###

Nesta pasta, deverão ser armazenados os arquivos necessários para se realizar um teste. Haverá no mínimo uma pasta chamada `config`, que conterá as informações básicas dos testes disponíveis.

Para listar quais testes estão disponíveis, adicione um arquivo chamado `versions.txt` dentro desta pasta `config`. Dentro deste arquivo, você deverá listar o código do teste (para poder organizar melhor os arquivos) e o nome do teste (para que você possa chamá-lo pelo programa). Isso deverá acontecer assim:

```
<código> <Nome do teste>
```

Por exemplo: vamos supor que queremos dois testes: um chamado "Infantil", que será codificado por "inf"; e outro chamado "Adulto", identificado por "adl". A versão final do arquivo `versions.txt` ficará assim:

```
inf Infantil
adl Adulto
```

Para cada um destes testes, deverão haver:

+ Um arquivo `<código>.txt`, contendo o nome das imagens e informações sobre elas.
+ Um arquivo `<código>.csv`, contendo uma tabela
+ Uma pasta chamada `<código>`, contendo as imagens do teste.

`TODO write about these files and this folder`.


### Pasta `results` ###

A pasta `results` é onde estarão listados os arquivos gerados pelo programa após a execução de um teste. Estes arquivos são tabelas do Excel que podem ser analisadas posteriormente.


## Criação de um teste

```
TODO WRITE ABOUT HOW TO CREATE A TEST
```


## Execução do programa

```
TODO WRITE ABOUT HOW THE APPLICATION RUNS
```