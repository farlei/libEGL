#pragma once

#include <vector>

class Simulador
{
protected:
	vector<RoboBase*> robos;
public:
	Simulador(void);
	~Simulador(void);

	void adicionaRobo(RoboBase* robo);
	void executaPasso();
};

