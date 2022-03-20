import {DateRange, DateRangePicker, LocalizationProvider} from '@mui/lab';
import AdapterMoment from '@mui/lab/AdapterMoment';
import {Box, Card, Divider, Stack, TextField, Typography, useTheme} from '@mui/material';
import moment from 'moment';
import {useEffect, useState} from 'react';
import ReactApexChart from 'react-apexcharts';
import Page from '../../layouts/Page';
import {bieuDoService} from '../../services';

interface ChartData {
  labels: string[];
  values: {name: string; type: 'column'; data: number[]}[];
}

function getDates(startDate: Date, stopDate: Date) {
  let dateArray = [];
  let currentDate = moment(startDate);
  let endDate = moment(stopDate);
  while (currentDate <= endDate) {
    dateArray.push(moment(currentDate).format('YYYY-MM-DD'));
    currentDate = moment(currentDate).add(1, 'days');
  }
  return dateArray;
}

const TongQuanPage = () => {
  const [dateRange, setDateRange] = useState<DateRange<Date>>([
    moment().day(-6).toDate(),
    moment().toDate(),
  ]);

  const [data, setData] = useState<ChartData>({
    labels: [],
    values: [],
  });
  const chartOptions: any = {
    ...BaseOptionChart(),
    stroke: {width: [0, 0, 0, 0]},
    plotOptions: {bar: {columnWidth: '11%', borderRadius: 4}},
    fill: {type: ['solid', 'solid', 'solid', 'solid']},
    labels: data.labels,
    xaxis: {
      // type: 'datetime',
      caterories: data.labels,
    },
    tooltip: {
      shared: true,
      intersect: false,
      y: {
        formatter: (y: number) => {
          if (typeof y !== 'undefined') {
            return `${y.toFixed(0)} voucher`;
          }
          return y;
        },
      },
    },
  };
  useEffect(() => {
    if (dateRange[0] && dateRange[1]) {
      bieuDoService
        .getGDTheoNgay({
          ngayBatDau: dateRange[0],
          ngayKetThuc: dateRange[1],
        })
        .then(res => {
          if (res) {
            let result: ChartData = {
              labels: [],
              values: [],
            };
            const date = getDates(dateRange[0] || new Date(), dateRange[1] || new Date());
            result.labels = date?.map(item => moment(item).format('DD/MM/YYYY')) || [];
            result.values = [
              {
                name: 'Chưa sử dụng',
                type: 'column',
                data:
                  date?.map(item => {
                    return (
                      res?.find(
                        i =>
                          moment(i?.ngay).format('YYYY-MM-DD') === moment(item).format('YYYY-MM-DD')
                      )?.chuaSuDung || 0
                    );
                  }) || [],
              },
              {
                name: 'Đã sử dụng',
                type: 'column',
                data:
                  date?.map(item => {
                    return (
                      res?.find(
                        i =>
                          moment(i?.ngay).format('YYYY-MM-DD') === moment(item).format('YYYY-MM-DD')
                      )?.daSuDung || 0
                    );
                  }) || [],
              },
              {
                name: 'Hủy',
                type: 'column',
                data:
                  date?.map(item => {
                    return (
                      res?.find(
                        i =>
                          moment(i?.ngay).format('YYYY-MM-DD') === moment(item).format('YYYY-MM-DD')
                      )?.huy || 0
                    );
                  }) || [],
              },
              {
                name: 'Hết hạn',
                type: 'column',
                data:
                  date?.map(item => {
                    return (
                      res?.find(
                        i =>
                          moment(i?.ngay).format('YYYY-MM-DD') === moment(item).format('YYYY-MM-DD')
                      )?.hetHan || 0
                    );
                  }) || [],
              },
            ];
            setData(result);
          }
        });
    } else {
      setData({
        labels: [],
        values: [],
      });
    }
  }, [dateRange]);
  return (
    <Page title="Tổng quan">
      <Card>
        <Stack direction="row">
          <Box>
            <Typography variant="h5" margin={2}>
              Biểu đồ giao dịch theo ngày
            </Typography>
          </Box>

          <LocalizationProvider dateAdapter={AdapterMoment} locale={'vi'}>
            <DateRangePicker
              startText="Từ ngày"
              toolbarPlaceholder="dd/mm/yyyy"
              endText="Đến ngày"
              value={dateRange}
              inputFormat={'DD/MM/YYYY'}
              onChange={newValue => {
                setDateRange(newValue);
              }}
              renderInput={(startProps, endProps) => (
                <>
                  <TextField {...startProps} margin="dense" InputLabelProps={{shrink: true}} />
                  <Box sx={{mx: 2}}> Đến </Box>
                  <TextField {...endProps} margin="dense" InputLabelProps={{shrink: true}} />
                </>
              )}
            />
          </LocalizationProvider>
        </Stack>
        <Divider />

        <Box sx={{p: 3, pb: 1}} dir="ltr">
          <ReactApexChart type="line" series={data.values} options={chartOptions} height={364} />
        </Box>
      </Card>
    </Page>
  );
};

function BaseOptionChart() {
  const theme = useTheme();

  const LABEL_TOTAL = {
    show: true,
    label: 'Total',
    color: theme.palette.text.secondary,
    ...theme.typography.subtitle2,
  };

  const LABEL_VALUE = {
    offsetY: 8,
    color: theme.palette.text.primary,
    ...theme.typography.h3,
  };

  return {
    // Colors
    colors: [
      theme.palette.info.main,
      theme.palette.success.main,
      theme.palette.error.main,
      theme.palette.warning.main,
      theme.palette.chart.blue[0],
      theme.palette.chart.violet[0],
      theme.palette.chart.green[0],
      theme.palette.chart.red[0],
    ],

    // Chart
    chart: {
      toolbar: {show: false},
      zoom: {enabled: false},
      // animations: { enabled: false },
      foreColor: theme.palette.text.disabled,
      fontFamily: theme.typography.fontFamily,
    },

    // States
    states: {
      hover: {
        filter: {
          type: 'lighten',
          value: 0.04,
        },
      },
      active: {
        filter: {
          type: 'darken',
          value: 0.88,
        },
      },
    },

    // Fill
    fill: {
      opacity: 1,
      // gradient: {
      //   type: 'vertical',
      //   shadeIntensity: 0,
      //   opacityFrom: 0.4,
      //   opacityTo: 0,
      //   stops: [0, 100],
      // },
    },

    // Datalabels
    dataLabels: {enabled: false},

    // Stroke
    stroke: {
      width: 3,
      curve: 'smooth',
      lineCap: 'round',
    },

    // Grid
    grid: {
      strokeDashArray: 3,
      borderColor: theme.palette.divider,
    },

    // Xaxis
    xaxis: {
      axisBorder: {show: false},
      axisTicks: {show: false},
    },

    // Markers
    markers: {
      size: 0,
      strokeColors: theme.palette.background.paper,
    },

    // Tooltip
    tooltip: {
      x: {
        show: false,
      },
    },

    // Legend
    legend: {
      show: true,
      fontSize: 13,
      position: 'top',
      horizontalAlign: 'right',
      markers: {
        radius: 12,
      },
      fontWeight: 500,
      itemMargin: {horizontal: 12},
      labels: {
        colors: theme.palette.text.primary,
      },
    },

    // plotOptions
    plotOptions: {
      // Bar
      bar: {
        columnWidth: '28%',
        borderRadius: 4,
      },
      // Pie + Donut
      pie: {
        donut: {
          labels: {
            show: true,
            value: LABEL_VALUE,
            total: LABEL_TOTAL,
          },
        },
      },
      // Radialbar
      radialBar: {
        track: {
          strokeWidth: '100%',
          background: theme.palette.grey[500],
        },
        dataLabels: {
          value: LABEL_VALUE,
          total: LABEL_TOTAL,
        },
      },
      // Radar
      radar: {
        polygons: {
          fill: {colors: ['transparent']},
          strokeColors: theme.palette.divider,
          connectorColors: theme.palette.divider,
        },
      },
      // polarArea
      polarArea: {
        rings: {
          strokeColor: theme.palette.divider,
        },
        spokes: {
          connectorColors: theme.palette.divider,
        },
      },
    },

    // Responsive
    responsive: [
      {
        // sm
        breakpoint: theme.breakpoints.values.sm,
        options: {
          plotOptions: {bar: {columnWidth: '40%'}},
        },
      },
      {
        // md
        breakpoint: theme.breakpoints.values.md,
        options: {
          plotOptions: {bar: {columnWidth: '32%'}},
        },
      },
    ],
  };
}

export default TongQuanPage;
