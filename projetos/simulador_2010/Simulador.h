#pragma once

#include <vector>

class Simulador
{
protected:
	vector<RoboBase*> robos;
	bool pausado;
	fonte texto;
	long ticks, startTicks;

public:
	Simulador(void);
	~Simulador(void);

	void contaTempo();
	void mostraTempo();
	void resetaTempo();

	void adicionaRobo(RoboBase* robo);
	void executaPasso();
	void pausa();
	void resume();
};

