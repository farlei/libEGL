libEGL 4
========

A didactic game programming library. 
Used to teach game programming and C++. 

by *Farlei Heinen*

Uso b�sico
----------

Utilize o projeto base_2010 que esta dentro da pasta projetos.
O projeto base_2010 tem todas as configura��es ajustadas para compilar usando a libEGL no Visual Studio 2010.

Ainda n�o existe um plugin ou script para a cria��o de novos projetos, voc� sempre deve utilizar o projeto base para iniciar.

Novidades na vers�o 4
---------------------

> todas as novidades podem ser testadas no projeto tanque_2010

* sistema para limitar o framerate, possibilitando que o jogo tenha uma velocidade uniforme em diferentes computadores. Por padr�o o framerate � limitado em 60 frames por segundo.
* novos comandos para permitir a depura��o de vari�veis: egl_depurar() 
* sistema de interface gr�fica (com bot�es e menus)
* desenho de ret�ngulos com os cantos arredondados
* desenho de linhas com possibilidade de determinar a espessura
* novo sistema de interface gr�fica: bot�es e itens de menu
* possibilidade de interceptar o bot�o de fechar janela (usando eventos da interface gr�fica)
* BUG: foi corrigido o bug de recorte de imagens. Agora � poss�vel carregar somente uma parte de uma imagem (testado somente em PNG)
* TileMap: agora a libEGL carrega arquivos exportados do aplicativo Tiled (http://www.mapeditor.org/) em formato JSON

> *Usando o Tiled:* para utilizar um mapa de tiles editado no Tiled, basta exportar uma vers�o no formato JSON. Para habilitar as colis�es, deve-se adicionar um layer chamado exatamente "egl_colide" e deixar sem tiles as �reas que N�O tem colis�o (ver exemplo do Tanque). Nessa vers�o as infos de cada tile ainda n�o s�o suportadas.
