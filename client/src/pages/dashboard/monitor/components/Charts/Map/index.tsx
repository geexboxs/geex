/* eslint-disable no-eval */
import { Scene, LineLayer, PointLayer, PolygonLayer } from '@antv/l7';
import { Mapbox } from '@antv/l7-maps';
import * as React from 'react';
import { Spin } from 'antd';

export default class Map extends React.Component<
  {},
  {
    loading: boolean;
  }
> {
  state: {
    loading: boolean;
  } = {
    loading: false,
  };

  private scene?: Scene;

  private initMap() {
    this.scene = new Scene({
      id: 'map',
      map: new Mapbox({
        pitch: 20,
        // @ts-ignore
        style: 'blank',
        center: [5, 40.16797],
        zoom: 0.51329,
        minZoom: 0.2,
      }),
    });
  }

  private addLayer() {
    this.setState({
      loading: true,
    });
    Promise.all([
      fetch(
        'https://gw.alipayobjects.com/os/basement_prod/dbd008f1-9189-461c-88aa-569357ffc07d.json',
      ).then((d) => d.json()),
      fetch(
        'https://gw.alipayobjects.com/os/basement_prod/4472780b-fea1-4fc2-9e4b-3ca716933dc7.json',
      ).then((d) => d.text()),
      fetch(
        'https://gw.alipayobjects.com/os/basement_prod/a5ac7bce-181b-40d1-8a16-271356264ad8.json',
      ).then((d) => d.text()),
    ]).then((res) => {
      requestAnimationFrame(() => {
        const [world, dot] = res;
        const dotData = eval(dot);

        this.setState({
          loading: false,
        });
        const worldFill = new PolygonLayer().source(world).color('#d1e0f3').shape('fill').style({
          opacity: 1,
        });

        const worldLine = new LineLayer().source(world).color('#fff').size(0.5).style({
          opacity: 0.4,
        });
        const dotPoint = new PointLayer()
          .source(dotData, {
            parser: {
              type: 'json',
              x: 'lng',
              y: 'lat',
            },
          })
          .shape('circle')
          .color('#268edc')
          .animate(false)
          .size(4)
          .style({
            opacity: 0.2,
          });

        this.scene && this.scene.addLayer(worldFill);
        this.scene && this.scene.addLayer(worldLine);
        this.scene && this.scene.addLayer(dotPoint);
      });
    });
  }

  public componentWillUnmount() {
    this.scene && this.scene.destroy();
  }

  public async componentDidMount() {
    this.initMap();
    this.addLayer();
  }

  public render() {
    const { loading } = this.state;
    return (
      <>
        {loading && <Spin />}
        <div
          id="map"
          style={{
            position: 'relative',
            width: '100%',
            height: '452px',
            display: loading ? 'none' : 'block',
          }}
        />
      </>
    );
  }
}
