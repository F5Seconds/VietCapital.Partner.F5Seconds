import {useLocation} from 'react-router-dom';
import {useAppSelector} from '../../redux/hooks';
import {selectQuyen} from '../../redux/slice/auth';

const useCheckQuyen = () => {
  const location = useLocation();
  const listQuyen = useAppSelector(selectQuyen);

  const checkQuyen = (quyen: 'seen' | 'create' | 'edit' | 'delete') => {
    const quyenEdit =
      listQuyen?.filter(item => item.split(';')[1] === quyen).map(item => item.split(';')[0]) || [];
    return quyenEdit.filter(item => location.pathname.includes(item)).length > 0;
  };

  return [checkQuyen];
};

export default useCheckQuyen;
