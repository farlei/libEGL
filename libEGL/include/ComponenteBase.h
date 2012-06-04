#pragma once
class ComponenteBase
{
protected:
	int x;
	int y;
	int largura;
	int altura;
	int id;
public:
	ComponenteBase(void);
	~ComponenteBase(void);
	int getID(void);
	bool dentroDoRetangulo(int mx, int my);
	virtual void desenha();
	virtual enum EGL_EVENTOS processaEventos();
};

