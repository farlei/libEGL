libEGL 4
========

A didactic game programming library. 
Used to teach game programming and C++. 

by *Farlei Heinen*

Uso básico
----------

Utilize o projeto base_2010 que esta dentro da pasta projetos.
O projeto base_2010 tem todas as configurações ajustadas para compilar usando a libEGL no Visual Studio 2010.

Ainda não existe um plugin ou script para a criação de novos projetos, você sempre deve utilizar o projeto base para iniciar.

Novidades na versão 4
---------------------

> todas as novidades podem ser testadas no projeto tanque_2010

* sistema para limitar o framerate, possibilitando que o jogo tenha uma velocidade uniforme em diferentes computadores. Por padrão o framerate é limitado em 60 frames por segundo.
* novos comandos para permitir a depuração de variáveis: egl_depurar() 
* sistema de interface gráfica (com botões e menus)
* desenho de retângulos com os cantos arredondados
* desenho de linhas com possibilidade de determinar a espessura
* novo sistema de interface gráfica: botões e itens de menu
* possibilidade de interceptar o botão de fechar janela (usando eventos da interface gráfica)
* BUG: foi corrigido o bug de recorte de imagens. Agora é possível carregar somente uma parte de uma imagem (testado somente em PNG)
* TileMap: agora a libEGL carrega arquivos exportados do aplicativo Tiled (http://www.mapeditor.org/) em formato JSON

> *Usando o Tiled:* para utilizar um mapa de tiles editado no Tiled, basta exportar uma versão no formato JSON. Para habilitar as colisões, deve-se adicionar um layer chamado exatamente "egl_colide" e deixar sem tiles as áreas que NÃO tem colisão (ver exemplo do Tanque). Nessa versão as infos de cada tile ainda não são suportadas.
