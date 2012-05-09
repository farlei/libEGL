#include <iostream>
#include <string>
#include <sstream>
#include <vector>
#include <algorithm>
#include "engcomp_glib.h"

using namespace std;

#include "TileEngine.h"

TileCache* TileCache::inst = 0;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Desenhar. </summary>
///
/// <remarks>	Tulio, 24/05/2010. </remarks>
///
/// <param name="index">	Zero-based index of the tile. </param>
/// <param name="x">		The x coordinate. </param>
/// <param name="y">		The y coordinate. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileCache::desenhar(int index,int x, int y)
{
	if(index < 0) return;
	if(index >= vetor.size()) return;

	vetor[index]->desenha(x,y);
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Carregar. </summary>
///
/// <remarks>	Tulio, 24/05/2010. </remarks>
///
/// <param name="arquivo">	Nome do arquivo. </param>
///
/// <returns>	Quantidade de tiles armazenados, -1 se falha. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileCache::carregar(string arquivo)
{
	//int pos = localizar(arquivo);
	//if(pos >= 0) return pos;

	imagem* temp;
	bool ret;

	temp = new imagem();
	ret = temp->carregar(arquivo);

	if(ret)
	{
		vetor.push_back(temp);
		mapa.push_back(arquivo);
		return vetor.size()-1;
	}
	else
	{
		delete temp;
		return -1;
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Localizar mapa pelo nome. </summary>
///
/// <remarks>	Tulio, 24/05/2010. </remarks>
///
/// <param name="arquivo">	Nome do arquivo do mapa. </param>
///
/// <returns>	Índice do mapa no Cache, -1 se não encontrar o mapa. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileCache::localizar(string arquivo)
{
	int sz = mapa.size();
	for(int i=0;i<sz;i++)
		if(mapa[i] == arquivo) return i;		
	
	return -1;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Obtém a imagem através do índice no Cache. </summary>
///
/// <remarks>	Tulio, 24/05/2010. </remarks>
///
/// <param name="index">	Zero-based index of the Cache. </param>
///
/// <returns>	Ponteiro para objeto imagem, 0 se falhar. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

imagem* TileCache::getImagem(int index)
{
	if(index < 0) return 0;
	if(index >= vetor.size()) return 0;

	return vetor[index];
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Gets the number tiles. </summary>
///
/// <remarks>	Tulio, 24/05/2010. </remarks>
///
/// <returns>	The number tiles. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileCache::getNumTiles()
{
	return vetor.size();
}

TileCache* TileCache::instance()
{
	if(!inst)
	{
		inst = new TileCache();
		return inst;
	}
	else return inst;
}
void TileCache::destruir()
{
	if(inst)
	{
		delete inst;
		inst = 0;
	}
}

TileCache::TileCache()
{
	// SINGLETON - construtor static
}
TileCache::~TileCache()
{
	for(int i = 0; i < vetor.size(); i++)
		delete vetor[i];
}