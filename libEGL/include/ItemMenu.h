#pragma once
class ItemMenu : public ComponenteBase
{
	int estado;
	int pressionado;
	string texto;
	bool background;
	fonte* fnt;
	int tx,ty;

	int txtLarg;
	int txtAlt;
	unsigned char corHover[3];
	unsigned char corNormal[3];
	unsigned char corFundoHover[3];
	unsigned char corFundoNormal[3];
public:
	ItemMenu(void);
	ItemMenu(int ID, string txt, int px, int py, int larg, int alt);
	~ItemMenu(void);

	void setarFonte(string arqFonte, int fntSize, bool fundo=false);

	void setarCorNormal(int tr, int tg, int tb, int br=0, int bg=0, int bb=0);
	void setarCorHover(int tr, int tg, int tb, int br=0, int bg=0, int bb=0);

	void desenha();
	enum EGL_EVENTOS processaEventos();
};

