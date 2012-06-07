#include <iostream>
#include <string>
#include <sstream>
#include <vector>
#include <fstream>
#include <math.h>
#include <algorithm>
#include <cctype>
#include "engcomp_glib.h"

using namespace std;

#include "TileEngine.h"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Default constructor. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
////////////////////////////////////////////////////////////////////////////////////////////////////

TileMap::TileMap(void)
{
	deltaGID = 0;
	px = py = 0;
	path = false;
	cutcor = false;
	movdiag = true;
	hr = HR_MANHATTAN;

	C = 10;
	CD = 14; // sqrt(2) * C
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Destructor. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
////////////////////////////////////////////////////////////////////////////////////////////////////

TileMap::~TileMap(void)
{
}



bool TileMap::inicializaJSON(string arq, string nomeLayer)
{
	Json::Value root;
	Json::Reader reader;
	ifstream inputArq(arq);
	bool ok = reader.parse(inputArq,root);
	if(!ok)
	{
		egl_erro("TileMap: Erro carregando formato JSON");
		return false;
	}

	deltaGID = iSC->getNumTiles()-1;

	lx = root.get("tilewidth",-1).asInt();
	ly = root.get("tileheight",-1).asInt();
	if((lx < 0) || (ly < 0))
	{
		egl_erro("TileMap: erro no JSON (tamanho do tile)");
		return false;
	}

	const Json::Value layers = root["layers"];
	bool achou = false;
	Json::Value layerAtivo;
	Json::Value layerColide;
	bool colisao = false;
	for(int i = 0; i < layers.size(); i++)
	{
		if(layers[i].get("name","").asString() == nomeLayer)
		{
			achou = true;
			layerAtivo = layers[i];
		}
		if(layers[i].get("name","").asString() == "egl_colide")
		{
			colisao = true;
			layerColide = layers[i];
		}
	}
	if(!achou)
	{
		egl_erro("TileMap: nome do layer nao foi encontrado");
		return false;
	}

	const Json::Value tilesets = root["tilesets"];
	for(int i = 0; i < tilesets.size(); i++)
	{
		int imgW = tilesets[i].get("imagewidth",0).asInt();
		int imgH = tilesets[i].get("imageheight",0).asInt();
		string imgName = tilesets[i].get("image","").asString();

		int xSize = imgW / lx;
		int ySize = imgH / ly;

		for(int posY = 0; posY < ySize; posY++)
		{
			for(int posX = 0; posX < xSize; posX++)
			{
				iSC->carregar(imgName,lx*posX,ly*posY,lx,ly);
			}
		}
	}

	tx = layerAtivo["width"].asInt();
	ty = layerAtivo["height"].asInt();

	mapaW = tx*lx;
	mapaH = ty*ly;

	mapa.resize(tx);
	for(int i = 0; i < tx; i++)
		mapa[i].resize(ty);

	int pos = 0;
	Json::Value tileArray = layerAtivo["data"];
	Json::Value colArray;
	if(colisao) colArray = layerColide["data"];
	for(int posY = 0; posY < ty; posY++)
	{
		for(int posX = 0; posX < tx; posX++)
		{
			int tile_atual = tileArray[pos].asInt();
			int tile_col = 0;

			mapa[posX][posY] = new Tiles();
			mapa[posX][posY]->posx = posX;
			mapa[posX][posY]->posy = posY;
			mapa[posX][posY]->setTile(tile_atual != 0 ? tile_atual+deltaGID : -1 ,lx,ly);

			// colisao: usando layer especial (egl_colide)
			if(colisao) tile_col = colArray[pos].asInt();
			if(tile_col != 0) mapa[posX][posY]->setWalk(false);
			else mapa[posX][posY]->setWalk(true);


			pos++;
		}
	}


	return true;
}



////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Inicializa. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="arq">	Arquivo que contém o mapa gerado pelo editor de mapas. </param>
///
/// <returns>	true if it succeeds, false if it fails. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

bool TileMap::inicializa(string arq)
{
	int delta;
	int tilesX, tilesY;
	int larg, alt;
	string tile_num;
	string nome_arq;
	tile_num = "*";

	ifstream arq_mapa(arq.c_str());
	if(!arq_mapa.is_open()) return false;

	arq_mapa >> tilesX >> tilesY;
	arq_mapa >> larg >> alt;

	delta = iSC->getNumTiles();

	arq_mapa >> tile_num;
	while(arq_mapa >> nome_arq)
	{
		iSC->carregar(nome_arq);
		arq_mapa >> tile_num;
		if(tile_num == "*") break;
	}
	
	if( (tilesX < 1) || (tilesY < 1)) return false;

	mapa.resize(tilesX);
	for(int i = 0; i < tilesX; i++)
		mapa[i].resize(tilesY);

	tx = tilesX; ty = tilesY;
	lx = larg; ly = alt;

	mapaW = tx*lx;
	mapaH = ty*ly;

	int tile_atual;
	for(int y = 0; y < tilesY; y++)
	{
		for(int x = 0; x < tilesX; x++)
		{
			arq_mapa >> tile_atual;
			if(tile_atual >= 0) tile_atual += delta;
			mapa[x][y] = new Tiles();
			mapa[x][y]->posx = x;
			mapa[x][y]->posy = y;
			mapa[x][y]->setTile(tile_atual+deltaGID,larg,alt);
		}
	}
	
	string sep;
	arq_mapa >> sep;
	if(sep != "*") return false;

	int bw_atual;
	for(int y = 0; y < tilesY; y++)
	{
		for(int x = 0; x < tilesX; x++)
		{
			arq_mapa >> bw_atual;
			mapa[x][y]->setWalk((bool)bw_atual);
		}
	}

	arq_mapa >> sep;
	if(sep != "*") return false;
	
	for(int y = 0; y < tilesY; y++)
	{
		for(int x = 0; x < tilesX; x++)
		{
			arq_mapa >> bw_atual;
			mapa[x][y]->setInfo(bw_atual);
		}
	}

	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Desenha o mapa. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::desenha()
{
	int wx, wy;
	wx = px;
	wy = py;
	for(int y = 0; y < ty; y++, wy+=ly)
	{
		if( (wy>-ly) && (wy<(res_y+ly)) )
			for(int x = 0; x < tx; x++, wx+=lx)
			{
				if( (wx>-lx) && (wx<(res_x+lx)) )
				{
					iSC->desenhar(mapa[x][y]->getTileN(),wx,wy);
				}
			}
		wx = px;
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Desenha o minimapa nas coordenadas x,y. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="_x">	The x coordinate. </param>
/// <param name="_y">	The y coordinate. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::desenha_minimapa(int _x, int _y)
{
	int minx = 2;
	int miny = 2;

	int wx, wy;
	wx = _x;
	wy = _y;
	for(int y = 0; y < ty; y++, wy+=miny)
	{
		for(int x = 0; x < tx; x++, wx+=minx)
		{
			if(mapa[x][y]->walkable)
			{
				egl_pixel(wx,wy,255,255,255);
				egl_pixel(wx+1,wy,255,255,255);
				egl_pixel(wx,wy+1,255,255,255);
				egl_pixel(wx+1,wy+1,255,255,255);
			}
			else
			{
				egl_pixel(wx,wy,0,0,0);
				egl_pixel(wx+1,wy,0,0,0);
				egl_pixel(wx,wy+1,0,0,0);
				egl_pixel(wx+1,wy+1,0,0,0);
			}

		}
		wx = _x;
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	TODO: descrição requerida. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="_x">	The x coordinate. </param>
/// <param name="_y">	The y coordinate. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::setPos(int _x, int _y)
{
	px = _x;
	py = _y;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Desloca o mapa na tela utilizando dx,dy posições. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="dx">	Variação no eixo x. </param>
/// <param name="dy">	Variação no eixo y. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::move(int dx, int dy)
{
	px += dx;
	py += dy;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	 Obtem o deslocamento no eixo X em relação a tela. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <returns>	deslocamente em relação a tela. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileMap::dX() 
{
	return px;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	 Obtem o deslocamento no eixo Y em relação a tela. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <returns>	deslocamente em relação a tela. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////
int TileMap::dY()
{
	return py;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Obtem a largura do mapa. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <returns>	Largura do mapa em colunas. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileMap::getW()
{
	return mapaW;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Obtem a altura do mapa. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <returns>	Altura do mapa em linha. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////
int TileMap::getH()
{
	return mapaH;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	TODO: _x e _y estão em qual unidade? Obtem um tile. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="_x">	Coordenada x. </param>
/// <param name="_y">	Coordenada y. </param>
///
/// <returns>	null if it fails, else the tile. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

Tiles* TileMap::getTile(int _x, int _y)
{
	return mapa[_x][_y];
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Screen 2map. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="x">	The x coordinate. </param>
/// <param name="y">	The y coordinate. </param>
/// <param name="mx">	[in,out] The mx. </param>
/// <param name="my">	[in,out] my. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::screen2map(int x, int y, int& mx, int& my)
{
	mx = x-px;
	my = y-py;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Map 2screen. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="x">	The x coordinate. </param>
/// <param name="y">	The y coordinate. </param>
/// <param name="mx">	[in,out] The mx. </param>
/// <param name="my">	[in,out] my. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::map2screen(int x, int y, int& mx, int& my)
{
	mx = x+px;
	my = y+py;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Xpara tela. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="mx">	The mx. </param>
///
/// <returns>	. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileMap::XparaTela(int mx)
{
	return mx+px;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Ypara tela. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="my">	my. </param>
///
/// <returns>	. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileMap::YparaTela(int my)
{
	return my+py;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Point col tile. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="x">	The x coordinate. </param>
/// <param name="y">	The y coordinate. </param>
/// <param name="cx">	[in,out] The cx. </param>
/// <param name="cy">	[in,out] The cy. </param>
///
/// <returns>	true if it succeeds, false if it fails. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

bool TileMap::pointColTile(int x,int y, int& cx, int& cy)
{
	if( (x<0) || (y<0) ) return false;
	if( (x >= tx*lx) || (y >= ty*ly) ) return false;
	cx = x/lx;
	cy = y/ly;
	return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Draw tile border. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="x">		The x coordinate. </param>
/// <param name="y">		The y coordinate. </param>
/// <param name="vermelho">	The vermelho. </param>
/// <param name="verde">	The verde. </param>
/// <param name="azul">		The azul. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::drawTileBorder(int x, int y, int vermelho, int verde, int azul)
{
	int x1,x2,y1,y2;
	x1 = (x*lx)+px; x2 = (x1+lx);
	y1 = (y*ly)+py; y2 = (y1+ly);

	egl_linha(x1,y1,x2,y1,vermelho,verde,azul);
	egl_linha(x2,y1,x2,y2,vermelho,verde,azul);
	egl_linha(x2,y2,x1,y2,vermelho,verde,azul);
	egl_linha(x1,y2,x1,y1,vermelho,verde,azul);
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Colides. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="x">	The x coordinate. </param>
/// <param name="y">	The y coordinate. </param>
/// <param name="w">	The width. </param>
/// <param name="h">	The height. </param>
///
/// <returns>	true if it succeeds, false if it fails. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

// colide: somente para sprites menores que os tiles
bool TileMap::colide(int x, int y, int w, int h)
{
	int tilx, tily;
	if(pointColTile(x,y,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk())) return true;
	}
	if(pointColTile(x+w,y,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk())) return true;
	}
	if(pointColTile(x,y+h,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk())) return true;
	}
	if(pointColTile(x+w,y+h,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk())) return true;
	}

	return false;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Colide pp. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="x">	The x coordinate. </param>
/// <param name="y">	The y coordinate. </param>
/// <param name="w">	The width. </param>
/// <param name="h">	The height. </param>
/// <param name="img">	[in,out] If non-null, the image. </param>
///
/// <returns>	true if it succeeds, false if it fails. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

//TODO: BUG se o mapa de colisao for maior que o tile
//TODO: bug com pngs transparentes
bool TileMap::colidePP(int x, int y, int w, int h, imagem* img) // Colide Pixel-a-Pixel
{
	imagem* img_tile;

	int tilx, tily;
	if(pointColTile(x,y,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk()))
		{
			img_tile = iSC->getImagem(mapa[tilx][tily]->getTileN());
			if(img->colide(x,y,tilx*lx,tily*ly,*img_tile)) 
				return true;
		}
	}
	if(pointColTile(x+w,y,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk())) 
		{
			img_tile = iSC->getImagem(mapa[tilx][tily]->getTileN());
			if(img->colide(x,y,tilx*lx,tily*ly,*img_tile)) 
				return true;
		}
	}
	if(pointColTile(x,y+h,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk())) 
		{
			img_tile = iSC->getImagem(mapa[tilx][tily]->getTileN());
			if(img->colide(x,y,tilx*lx,tily*ly,*img_tile)) 
				return true;
		}
	}
	if(pointColTile(x+w,y+h,tilx,tily) )
	{
		if(!(mapa[tilx][tily]->getWalk())) 
		{
			img_tile = iSC->getImagem(mapa[tilx][tily]->getTileN());
			if(img->colide(x,y,tilx*lx,tily*ly,*img_tile)) 
				return true;
		}
	}


	return false;
}

//////////////////////////
// ASTAR
//////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Desenha closed. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::desenhaClosed()
{
	int wx, wy;
	wx = px;
	wy = py;
	char txt[32];
	for(int y = 0; y < ty; y++, wy+=ly)
	{
		if( (wy>-ly) && (wy<(res_y+ly)) )
			for(int x = 0; x < tx; x++, wx+=lx)
			{
				if( (wx>-lx) && (wx<(res_x+lx)) )
				{
					if( (mapa[x][y]->inClosed) || (mapa[x][y]->inOpen) )
					{
						drawTileBorder(x,y);
						_itoa_s(mapa[x][y]->F,txt,10);
						egl_texto(txt,wx+2,wy+2);
						_itoa_s(mapa[x][y]->G + mapa[x][y]->getAdditionalCost() ,txt,10);
						egl_texto(txt,wx+2,wy+11);
						_itoa_s(mapa[x][y]->H,txt,10);
						egl_texto(txt,wx+2,wy+21);
					}
				}
			}
		wx = px;
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Heuristicas. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="ix">	The ix. </param>
/// <param name="iy">	The iy. </param>
/// <param name="fx">	The fx. </param>
/// <param name="fy">	The fy. </param>
///
/// <returns>	. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileMap::Heuristica(int ix,int iy,int fx,int fy)
{
	switch(hr)
	{
		default:
		case HR_MANHATTAN :  
			return ( C * (abs(ix-fx) + abs(iy-fy)) );
		case HR_EUCLIDEAN : 
			return ( C * sqrt(pow(ix-fx,2.0) + pow(iy-fy,2.0)) );
		case HR_DIAGONAL:
		{
			int h_diagonal = min(abs(ix-fx), abs(iy-fy));
			int h_straight = (abs(ix-fx) + abs(iy-fy));
			return CD * h_diagonal + C * (h_straight - 2*h_diagonal);
		}
		case HR_CROSS:
		{
			int heu = C * (abs(ix-fx) + abs(iy-fy));
			int cross = abs((ix - fx)*(sy - fy) - (sx - fx)*(iy - fy));
			return (heu + cross)/2.0;
		}
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Menor f. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <returns>	null if it fails, else. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

Tiles* TileMap::MenorF()
{
	if(open.empty()) return NULL; 

	int mF;
	Tiles* mT = NULL;
	list<Tiles*>::iterator mit;
	list<Tiles*>::iterator it;
	it = open.begin();
	
	mF = (*it)->F;
	mT = (*it);
	mit = it;
	it++;
	
	while(it != open.end())
	{
		if( (*it)->F < mF)
		{
			mF = (*it)->F;
			mT = (*it);
			mit = it;
		}
		it++;
	}
	open.erase(mit);
	mT->inOpen = false;

	closed.push_back(mT);
	mT->inClosed = true;
	
	if( (mT->posx == gx) && (mT->posy == gy) ) path = true;
	return mT;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Cut corner. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="ax">	The ax. </param>
/// <param name="ay">	The ay. </param>
/// <param name="bx">	The bx. </param>
/// <param name="by">	The by. </param>
///
/// <returns>	true if it succeeds, false if it fails. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

// Evita Cortar por Cantos fechados : se move na diagonal E possui alguma adjacente nas paralelas
// ax,ay = origem
// bx,by = destino
bool TileMap::CutCorner(int ax, int ay, int bx, int by)
{
	if(cutcor) return false;

	if (bx == ax-1) 
	{
		if (by == ay-1)
		{
			
			if (mapa[ax-1][ay]->walkable == false || mapa[ax][ay-1]->walkable == false)
				return true;
		}
		else 
			if (by == ay+1)
			{
				if (mapa[ax][ay+1]->walkable == false || mapa[ax-1][ay]->walkable == false) 
					return true;
			}
	}
	else 
		if (bx == ax+1)
		{
			if (by == ay-1)
			{
				if (mapa[ax][ay-1]->walkable == false || mapa[ax+1][ay]->walkable == false) 
					return true;
			}
			else 
				if (by == ay+1)
				{
					if (mapa[ax+1][ay]->walkable == false || mapa[ax][ay+1]->walkable == false)
						return true;
				}
		}	
	return false;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Processa adjacente. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="adx">		The adx. </param>
/// <param name="ady">		The ady. </param>
/// <param name="G">		The. </param>
/// <param name="atual">	[in,out] If non-null, the atual. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::ProcessaAdjacente(int adx, int ady, int G, Tiles* atual)
{
	Tiles* adT;

	if( (adx>=0) && (adx<tx) && (ady>=0) && (ady<ty))
	{
		if( !CutCorner(atual->getX(),atual->getY(),adx,ady) )
		{
			adT = mapa[adx][ady];
			G = (G + atual->G) + adT->cost;
			if( adT->walkable && !(adT->inClosed))
			{
				if(!adT->inOpen)
				{
					adT->anterior = atual;
					adT->G = G;
					adT->H = Heuristica(adx,ady,gx,gy);
					adT->F = adT->H + G;
					adT->inOpen = true;
					open.push_back(adT);
				}
				else
				{
					if( G < adT->G)
					{
						adT->anterior = atual;
						adT->G = G;				
						adT->F = adT->H + G;
					}
				}
			}
		}
	}

}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Adjacentes. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="atual">	[in,out] If non-null, the atual. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::Adjacentes(Tiles* atual)
{
	int adx,ady,G;
	int ax = atual->posx;
	int ay = atual->posy;
	
	// G: custo do movimento
	//  10 = movimento normal - C
	//  14 = movimento diagonal - CD
	if(movdiag)
	{
		G = CD + atual->getAdditionalCost();
	}
	else
	{
		G = C + atual->getAdditionalCost();
	}
	adx = ax; ady = ay-1; /*G = C;*/
	ProcessaAdjacente(adx,ady,G,atual);

	adx = ax+1; ady = ay; /*G = C;*/
	ProcessaAdjacente(adx,ady,G,atual);
	
	adx = ax; ady = ay+1; /*G = C;*/
	ProcessaAdjacente(adx,ady,G,atual);
	
	adx = ax-1; ady = ay; /*G = C;*/
	ProcessaAdjacente(adx,ady,G,atual);

	if(movdiag)
	{
		adx = ax-1; ady = ay-1; /*G = CD;*/
		ProcessaAdjacente(adx,ady,G,atual);
		
		adx = ax+1; ady = ay-1; /*G = CD;*/
		ProcessaAdjacente(adx,ady,G,atual);

		adx = ax+1; ady = ay+1; /*G = CD;*/
		ProcessaAdjacente(adx,ady,G,atual);
		
		adx = ax-1; ady = ay+1; /*G = CD;*/
		ProcessaAdjacente(adx,ady,G,atual);
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Passoes this object. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <returns>	true if it succeeds, false if it fails. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

bool TileMap::Passo()
{
	Tiles* atual;

	// acha o menor F e retira da lista de open, colocando na lista de closed
	atual = MenorF();
	if( (!path) && (atual) )
	{
		Adjacentes(atual);
		return false;
	}
	else
		return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Limpa caminho. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::LimpaCaminho()
{
	list<Tiles*>::iterator it;

	it = closed.begin();
	while(it != closed.end())
	{
		(*it)->inClosed = false;
		it++;
	}

	it = open.begin();
	while(it != open.end())
	{
		(*it)->inOpen = false;
		it++;
	}

	path = false;
	closed.clear();
	open.clear();
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Calcula caminho. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="ix">	The ix. </param>
/// <param name="iy">	The iy. </param>
/// <param name="fx">	The fx. </param>
/// <param name="fy">	The fy. </param>
///
/// <returns>	true if it succeeds, false if it fails. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

// ix,iy = inicio
// fx,fy = destino
bool TileMap::CalculaCaminho(int ix,int iy,int fx,int fy)
{
	// valida posicoes de inicio e fim
	if( (ix < 0) || (ix >= tx) ) return false; if( (iy < 0) || (iy >= ty) ) return false;
	if( (fx < 0) || (fx >= tx) ) return false; if( (fy < 0) || (fy >= ty) ) return false;

	// inicializa as estruturas de dados
	LimpaCaminho();

	// seta o inicio;
	sx = ix; sy = iy;

	// seta o destino
	gx = fx; gy = fy;
	

	// se o inicio não for walkable não existe caminho. retorna imediatamente
	if( !(mapa[ix][iy]->walkable) ) return path;

	// inicia a procura a partir do inicio
	mapa[ix][iy]->anterior = NULL;
	mapa[ix][iy]->G = 0;
	mapa[ix][iy]->H = Heuristica(ix,iy,fx,fy);
	mapa[ix][iy]->F = mapa[ix][iy]->H;
	mapa[ix][iy]->inOpen = true;
	open.push_back(mapa[ix][iy]);

	// executa a busca, passo a passo
	while(!Passo())
	{
		if(open.empty()) return false;
	}

	return path;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Gets the caminho. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <returns>	null if it fails, else the caminho. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

// retorna um vetor com os tiles que fazem parte do caminho
vector<Tiles*> TileMap::GetCaminho()
{
	vector<Tiles*> cam;

	if(path)
	{
		// faz um traceback do destino até o inicio, usando o ponteiro do anterior
		Tiles* atual = mapa[gx][gy];
		while(atual)
		{
			cam.push_back(atual);
			atual = atual->anterior;
		}
	}

	return cam;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Sets a s tar options. </summary>
///
/// <remarks>	Tulio, 25/05/2010. </remarks>
///
/// <param name="heuristic">	The heuristic. </param>
/// <param name="cutcorner">	true to cutcorner. </param>
/// <param name="move_diag">	true to move diagram. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void TileMap::setASTarOptions(int heuristic, bool cutcorner,bool move_diag)
{
	hr = heuristic;
	cutcor = cutcorner;
	movdiag = move_diag;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Gets the tile width. </summary>
///
/// <remarks>	Tulio, 08/06/2010. </remarks>
///
/// <returns>	The tile w. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileMap::getTileW()
{
	return lx;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Gets the tile height. </summary>
///
/// <remarks>	Tulio, 08/06/2010. </remarks>
///
/// <returns>	The tile h. </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////

int TileMap::getTileH()
{
	return ly;
}
