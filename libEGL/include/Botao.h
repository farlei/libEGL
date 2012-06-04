#pragma once
class Botao : public ComponenteBase
{
private:
	int estado;
	imagem imgs[3];
	int pressionado;
public:
	Botao(void);
	Botao(int ID, int px, int py, string imgNormal, string imgClique="", string imgHover="");
	~Botao(void);

	void desenha();
	enum EGL_EVENTOS processaEventos();

};

