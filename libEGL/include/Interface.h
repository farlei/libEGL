#pragma once

enum EGL_EVENTOS {EGL_VAZIO, EGL_CLIQUE, EGL_HOVER, EGL_EDITADO};

class Interface
{
private:
	vector<ComponenteBase*> componentes;
public:
	Interface(void);
	~Interface(void);

	void desenha(void);
	ComponenteBase* buscaComponente(int compID);
	void adicionaComponente(ComponenteBase* componente);
	int verificaEventos(enum EGL_EVENTOS tipo = EGL_CLIQUE);
};

